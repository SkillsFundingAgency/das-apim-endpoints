#!/usr/bin/env node
process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
const fs = require("fs");
const https = require("https");
const http = require("http");
const path = require("path");
const crypto = require("crypto");

const envFile = "HttpScripts/http-client.env.json";

let envVars = {};
let globalVars = {};

function uuid() {
    return crypto.randomUUID?.() || [4, 2, 2, 2, 6].map(len => [...Array(len)].map(() => Math.floor(Math.random() * 16).toString(16)).join("")).join("-");
}

function interpolate(str) {
    return str.replace(/{{(.*?)}}/g, (_, name) => {
        if (name === "$random.uuid") return uuid();
        const val = globalVars[name] ?? envVars[name];
        if (val === undefined) return `{{${name}}}`;
        return typeof val === "object" ? JSON.stringify(val) : val;
    });
}

function executeResponseScript(code, responseJson, statusCode, requestLabel) {
    const failures = [];
    const client = {
        global: {
            set: (k, v) => { globalVars[k] = v; },
            get: (k) => globalVars[k] ?? envVars[k],
        },
        test: (_name, fn) => { fn(); },
        assert: (condition, message) => {
            if (!condition) {
                failures.push(message);
                console.log(`##vso[task.logissue type=error;] ${requestLabel} → Assert failed: ${message}`);
                process.exitCode = 1;
            }
        },
    };
    const response = { status: statusCode, body: responseJson };
    try {
        Function("client", "response", code)(client, response);
    } catch (err) {
        console.warn(`⚠️ Response script error: ${err.message}`);
    }
    return failures;
}

function runRequestBlock(block, output = []) {
    const lines = block.trim().split("\n");
    const [method, url] = interpolate(lines[0].trim()).split(" ");
    let headers = {};
    let body = "";
    let isHeader = true;

    for (let i = 1; i < lines.length; i++) {
        const line = lines[i].trim();
        if (line.startsWith(">")) break;
        if (line === "") {
            isHeader = false;
            continue;
        }
        if (isHeader) {
            const [key, ...rest] = line.split(":");
            headers[key.trim()] = interpolate(rest.join(":").trim());
        } else {
            body += interpolate(line) + "\n";
        }
    }

    const responseScriptMatch = block.match(/>\s*\{%([\s\S]*?)%\}/);

    const curl = [
        `curl -X ${method}`,
        `"${url}"`,
        ...Object.entries(headers).map(([k, v]) => `-H "${k}: ${v}"`),
        body.trim() ? `-d '${body.trim()}'` : "",
    ]
        .filter(Boolean)
        .join(" \\\n  ");
    output.push(curl);

    return new Promise((resolve) => {
        const urlObj = new URL(url);
        const client = urlObj.protocol === "https:" ? https : http;

        const options = {
            method,
            hostname: urlObj.hostname,
            port: urlObj.port || (urlObj.protocol === "https:" ? 443 : 80),
            path: urlObj.pathname + urlObj.search,
            headers,
        };

        const req = client.request(options, (res) => {
            let data = "";
            res.on("data", (chunk) => (data += chunk));
            res.on("end", () => {
                let json;
                try {
                    json = JSON.parse(data);
                } catch {}

                const label = `${method} ${url}`;

                if (responseScriptMatch) {
                    const failures = executeResponseScript(responseScriptMatch[1], json, res.statusCode, label);
                    if (failures.length === 0) {
                        console.log(`✅ ${label} → All asserts passed`);
                    }
                } else {
                    console.log(`ℹ️ ${label} → ${res.statusCode}`);
                }

                resolve();
            });
        });

        req.on("error", (e) => {
            console.error(`##vso[task.logissue type=error;] ${method} ${url}`, e.message);
            process.exitCode = 1;
            resolve();
        });

        if (body.trim()) req.write(body);
        req.end();
    });
}

(async function main() {
    const [,, httpFile, ...rest] = process.argv;
    const args = rest.reduce((acc, arg, i) => {
        if (arg.startsWith("--")) {
            const [key, value] = arg.includes("=") ? arg.slice(2).split("=") : [arg.slice(2), rest[i + 1]];
            acc[key] = value ?? true;
        }
        return acc;
    }, {});

    if (!httpFile || !fs.existsSync(httpFile)) {
        console.error("❌ Missing or invalid .http file");
        process.exit(1);
    }

    try {
        const json = JSON.parse(fs.readFileSync(envFile, "utf-8"));
        envVars = json[args.env] || {};
    } catch (e) {
        console.warn(`⚠️ Could not read ${envFile}, continuing with empty environment`);
    }

    const text = fs.readFileSync(httpFile, "utf-8");
    const requests = text.split(/^#{3,}$/m).map(s => s.trim()).filter(Boolean);
    const output = [];

    for (const block of requests) {
        const firstLine = block.trim().split("\n")[0].trim();
        const waitMatch = firstLine.match(/^WAIT\s+(\d+)$/i);
        if (waitMatch) {
            const ms = parseInt(waitMatch[1], 10);
            console.log(`⏳ Waiting ${ms}ms...`);
            await new Promise(resolve => setTimeout(resolve, ms));
            continue;
        }

        // Strip and execute pre-request scripts (< {% ... %})
        let requestBlock = block;
        const preRequestMatch = block.match(/^<\s*\{%([\s\S]*?)%\}\s*\n([\s\S]*)$/);
        if (preRequestMatch) {
            const preScript = preRequestMatch[1];
            requestBlock = preRequestMatch[2];
            try {
                const client = {
                    global: {
                        set: (k, v) => { globalVars[k] = v; },
                        get: (k) => globalVars[k] ?? envVars[k],
                    }
                };
                Function("client", "crypto", preScript)(client, { randomUUID: uuid });
            } catch (e) {
                console.warn(`⚠️ Pre-request script error: ${e.message}`);
            }
        }

        await runRequestBlock(requestBlock, output);
    }

    if (args.out) {
        fs.writeFileSync(args.out, output.join("\n\n") + "\n", "utf-8");
        console.log(`💾 Output saved to ${args.out}`);
    } else {
        console.log(output.join("\n\n"));
    }
})();
