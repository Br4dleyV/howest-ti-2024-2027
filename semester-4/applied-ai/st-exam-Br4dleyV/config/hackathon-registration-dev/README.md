# Hackathon Registration Dev Environment

This folder contains the supporting services used by the exam project during local development. These services are not the Deno assistant application itself. Instead, they provide the existing backend that your assistant must call when it has successfully parsed and validated a handwritten registration form.

## What This Environment Provides

The Docker Compose setup starts two services:

1. `hackathon-registration`: the provided Hackathon Registration Web API.
2. `hackathon-registration-db`: the PostgreSQL database used by that API.

Together they form the system of record for hackathon groups. Your assistant should not bypass them. For successful registrations, it should use the existing API or the MCP tools that wrap that API.

## Hackathon Registration Web API

The provided Web API is available through Docker Compose on:

- `http://localhost:40520`

Its purpose is straightforward: manage hackathon groups and their members. In the context of this exam, your assistant is responsible for turning a handwritten registration form into a valid group registration request that this API can accept.

### Main Goal

The API exists to store and retrieve hackathon registrations in a structured way. It is the backend your assistant depends on after it has completed these steps:

1. determine whether the uploaded image is really a hackathon registration form;
2. extract the group name, goal, and members;
3. reject incomplete or invalid forms;
4. submit valid registrations without inventing missing data.

### Main Functionality

Based on the provided example requests, the API supports these group-oriented operations:

1. `POST /groups`
Creates a new group with a name, a goal, and a member list.

2. `GET /groups`
Lists the registered groups.

3. `GET /groups/goals`
Lists the supported or known goals.

4. `GET /groups/{id}`
Fetches a single group by its numeric identifier.

5. `PUT /groups/{id}`
Updates an existing group and its members.

### Expected Validation Behavior

The example scenarios show that the API enforces business rules. In particular:

- a group must contain a valid name;
- a group must contain a valid goal;
- a group must contain between 2 and 5 members;
- invalid identifiers or unknown group ids are rejected.

That matters because your assistant should prevent bad mutations before calling the backend. The backend still validates, but your agent should avoid calling it for obviously invalid forms.

## Database

The registration API stores its data in a PostgreSQL database provided by the `hackathon-registration-db` service.

### Purpose

This database is the persistence layer for the provided registration backend. It holds the registered groups and related data so that registrations survive across API calls.

### Local Development Configuration

The database container is configured through `config/hackathon-registration-dev/.hackathon-registration-db.env` and `config/hackathon-registration-dev/.hackathon-registration.env`.

Relevant settings are:

- database name: `exam_db`
- database user: `exam_user`
- database password: `exam_password`
- container database port: `5000`
- host port mapping: `40521`

The API container connects to the database internally using:

- host: `hackathon-registration-db`
- port: `5000`

The database data is persisted in the Docker volume named `postgres`.

## Provided MCP Server

The repository also includes a ready-to-use MCP server definition in `hackathon-catalog.yaml`.

### Purpose

This MCP server exposes the Hackathon Registration API as agent-friendly tools. Instead of making raw HTTP calls from your IDE agent, the agent can invoke well-named tools that already map to the backend's group operations.

## How The Pieces Fit Together

In local development, the overall flow is:

1. Docker Compose starts the provided registration API and its PostgreSQL database.
2. The MCP server connects to that API and exposes its operations as tools for an agent.
3. Your IDE agent or your Google ADK based application agent analyzes the handwritten form.
4. If the form is valid, the agent uses the registration backend, directly or through MCP tools, to create or update group data.
5. The backend persists the result in PostgreSQL.
