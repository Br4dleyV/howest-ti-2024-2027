# Applied AI Exam: Hackathon Registration Assistant

- Module: Applied AI
- Teachers: Matthias Blomme, Dimitri Casier
- Date: 02/04/2026, 14u00 - 16u30

## Rules

1. All AI usage is allowed during this exam.
2. You may use AI tools for research, prompting, code generation, debugging, and documentation.
3. All course materials are allowed to be used during the exam.
4. You may not contact or talk to other students or any other persons during the exam.
5. The work you submit must remain your own work, completed individually within these rules.

## Overview

In this exam you will turn the starter repository into a hackathon registration **assistant** that can process a handwritten registration form.

You will implement the same business capability in two different ways:

**1. Part I**: configure your IDE AI Agent so it can handle a handwritten hackathon registration form.

**2. Part II**: build your own Google ADK based agent inside this application so it can do the same work..

The end goal is a system that can:

1. determine whether an uploaded image is a hackathon registration form;
2. extract the registration data from the image;
3. use self-defined hackathon registration FunctionTools or skills;
4. finish with a concise confirmation.

## Scenario

The handwritten form `hackathon-registration-form.jpg` contains the data needed to register a hackathon group. A correct solution should be able to extract at least these fields:

1. group name;
2. goal;
3. members, with first name and last name.

Then depending on the part:
- for Part I, the IDE agent should be able to call the hackathon registration API through skills
- for Part II, the Google ADK agent should be able to call the hackathon registration API through FunctionTools you implement.


## Provided hackathon registration API

### Common tools for both part I and II
A working web API called `hackathon-registration`.
- The Web API is avaialable on localhost:40520 and is documented in `config/hackathon-registration-dev/docker-compose.yml` and `config/hackathon-registration-dev/example-requests/hackathon-registration-scenarios.http`.
- Sample handwritten form image in `hackathon-registration-form.jpg`;

### Additional for Part II

This repository already contains the main boundaries of the assignment:

1. an OpenAPI contract in `config/openapi.yml` with the endpoint you need to implement for Part II;
2. a smoke test script in `verify_scan.sh`;
3. a starter controller that already accepts the uploaded image and converts it to base64;
4. an application agent folder in `agents/hackathon-registration-assistant/` for Part II;
5. domain abstractions and dependency injection modules for the Google ADK implementation.

## Prerequisites

Before you start:

1. take a look at `config/hackathon-registration-dev/README.md` to know what the hackathon registration Web API is all about.
2. copy `config/.env.example` to `config/.env`;
3. set a valid `GOOGLE_API_KEY` in `config/.env`;
4. bootstrap the supporting services with Docker Compose from `config/hackathon-registration-dev/docker-compose.yml`;
5. make sure you can run the project locally with `deno task run`;
6. use the example requests in `config/hackathon-registration-dev/example-requests/hackathon-registration-scenarios.http` to confirm the supporting services are running and working correctly;
7. make sure the hackathon registration Web API is reachable on localhost:40520 so your can call it.


## Part I: Configure Your IDE AI Agent

### Objective

Configure your IDE agent so that it can handle a handwritten hackathon registration form end to end.

## Repository Orientation

Use these areas as your main entry points:

1. `.vscode/` and `.agents/skills/`

### Your Work

Complete the agent configuration and prompting so that the agent can:

1. inspect an attached image of a handwritten form;
2. decide whether the image is a hackathon registration form;
3. extract the group name, goal, and members;
4. use a skill that you define to call the hackathon registration API;
5. produce a concise confirmation message for successful processing.

For this part, create the IDE integration pieces and the required skills.

### Screenshot Requirement

> You must provide a clean screenshot that shows your IDE agent handling the registration form.
>
> Name: **part1_screenshot.jpg**

The only prompt that is allowed to the IDE Agent is `handle`.

Your screenshot should clearly show:

1. the prompt `handle`;
2. the IDE agent response that ends with a clear registration report.
3. save the screenshot with the name `part1_screenshot.jpg` and include it in the main branch.

## Part II: Build The Same Capability With Google ADK

> **Warning**: Build this solution with model `gemini-2.5-flash`.
> 
> Version `gemini-3.0` will be used for evaluation.

### Objective

Implement the same handwritten form handling flow inside the application by building your own Google ADK based agent.

### Your Work

For Part II, complete the missing application and infrastructure flow so that the API endpoint can trigger the Google ADK agent.

Your implementation must:

1. keep the public API contract defined in `config/openapi.yml`;
2. use the existing `ScanHackathonRegistrationFormController` as the web entry point;
3. add the missing application layer orchestration for the scan flow;
4. implement the missing Google ADK infrastructure in `src/Infrastructure/AI/GoogleADK/`;
5. use the application agent configuration in `agents/hackathon-registration-assistant/`;
6. support multimodal prompting so the uploaded image reaches the model;
7. create the FunctionTools needed to expose the hackathon registration Web API to the ADK agent;

## Repository Orientation

Use these areas as your main entry points:

2. `agents/hackathon-registration-assistant/`: Part II application agent configuration and prompts.
3. `src/Infrastructure/WebApi/Controllers/ScanHackathonRegistrationFormController.ts`: HTTP entry point for Part II.
4. `src/Domain/Agents/` and `src/Domain/Prompts/`: existing abstractions you should respect.
5. `src/Main/Modules/Application/Module.ts`, `src/Main/Modules/AI/Module.ts`, and `src/Main/Modules/WebApi/WebApiControllerFactory.ts`: dependency injection wiring.
6. `src/Infrastructure/AI/GoogleADK/`: infrastructure area intended for the Google ADK implementation.

### Expected Outcome

At the end of Part II, a POST request to `/api/assistant/scan-hackathon-registration-form` should trigger your Google ADK based implementation and complete the handwritten form handling flow.

The HTTP interface must remain aligned with the provided OpenAPI description. Internally, the request should go through the existing port and adapters layers, and the ADK agent should reach the hackathon registration system through FunctionTools you implemented.

### Teacher Validation

For Part II, the evaluation is not limited to code quality.

The teacher will also script calls against your scan endpoint and will fetch registrations through the available hackathon registration Web API endpoints.

Because of that, do not change the network configuration or credentials in the `config/` folder. Keep the provided environment and integration setup compatible with the supplied Docker Compose configuration, example requests, and API contracts.

 - Live evaluation

When you are done with Part II, raise your hand and the teacher will run a live evaluation by sending a POST request to your scan endpoint with the provided handwritten form image. The teacher will also check the registered group through the hackathon registration Web API.

The following checks will be performed:
- The students shows a database with empty groups before the scan
- The students calls the POST request to the scan endpoint with the image attached through the swagger UI or a similar tool
- The teacher has a clear view on the terminal logs of the application during the scan.
- The teacher checks the database after the scan to confirm a new group has been registered with the correct data.


## Verification

Use the following checks before you submit:

1. run the application with `deno task run`;
2. test the API flow with `bash verify_scan.sh`;
3. verify that the endpoint still returns the success response documented in `config/openapi.yml` when processing succeeds;
4. verify from your IDE that the configured agent can process `hackathon-registration-form.jpg`;
5. there is a screenshot named `part1_screenshot.jpg` on the root of the main branch.
