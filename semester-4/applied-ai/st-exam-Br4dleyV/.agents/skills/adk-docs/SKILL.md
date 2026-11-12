# Skill: adk-docs

## Purpose
Provide implementation guidance for Agent Development Kit (ADK) in TypeScript for this repository.

## Scope
- Repository-only. Use local docs and code in this repo.
- Primary source: google_adk_docs.txt
- Secondary sources: agents/* prompts, src/Infrastructure/AI/GoogleADK/*, deno.json

## When to Use
- User asks how to implement an ADK feature in TypeScript.
- User asks how to wire ADK behavior into this repository.

## How to Use (Process)
1. Read google_adk_docs.txt and locate the TypeScript quickstart or relevant ADK section.
2. Summarize the relevant ADK API pieces (e.g., FunctionTool, LlmAgent, devtools).
3. Map the guidance to this repo’s Deno + TypeScript constraints and hexagonal architecture.
4. Provide concrete, repo-aware steps or code changes that fit the existing structure.

## Output Expectations
- Short, actionable guidance.
- Prefer repository paths and conventions.
- If a direct mapping is not possible, explicitly call it out and suggest closest alternative.
