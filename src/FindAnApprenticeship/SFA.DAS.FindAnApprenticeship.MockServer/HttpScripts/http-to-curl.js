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
        return globalVars[name] ?? envVars[name] ?? `{{${name}}}`;
    });
}

function extractVariable(code, responseJson) {
    const matches = [...code.matchAll(/client\.global\.set\("(.+?)",\s*response\.body\.(.+?)\)/g)];
    for (const [, varName, jsExpr] of matches) {
        try {
            // Build a function that evaluates the JS expression safely
            const value = Function("body", `return body.${jsExpr}`)(responseJson);
            if (value !== undefined) {
                globalVars[varName] = value;
            }
        } catch (err) {
            console.warn(`⚠️ Failed to extract '${varName}' from response.body.${jsExpr}`);
        }
    }
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
                try {
                    const json = JSON.parse(data);
                    if (block.includes("client.global.set")) {
                        extractVariable(block, json);
                        console.log("📦 Captured vars:", globalVars);
                    }
                } catch {}
                console.log(`✅ ${method} ${url} → ${res.statusCode}`);
                resolve();
            });
        });

        req.on("error", (e) => {
            console.error(`❌ Request failed: ${method} ${url}`, e.message);
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
        if (args.exec) {
            await runRequestBlock(block, output);
        } else {
            const lines = block.trim().split("\n");
            const curlCommand = await runRequestBlock(block, []);
            output.push(curlCommand);
        }
    }

    if (args.out) {
        fs.writeFileSync(args.out, output.join("\n\n") + "\n", "utf-8");
        console.log(`💾 Output saved to ${args.out}`);
    } else {
        console.log(output.join("\n\n"));
    }
})();
