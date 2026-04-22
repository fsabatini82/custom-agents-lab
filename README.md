# Training Repo — Custom Agents Crew

> Build your own GitHub Copilot agents and skills in 60 minutes.

In this hands-on workshop you will create a small **crew of AI agents** that live inside your repository and automate two recurring tasks: **code review** and **project documentation**. By the end of the session you will have a working pipeline that reviews C# code against your standards and generates branded HTML documentation from your codebase — all driven by markdown files under `.github/`.

---

## What's in this repo

Two folders, two roles:

| Folder | Purpose | You will… |
|--------|---------|-----------|
| [`lab-starter/`](./lab-starter) | Scaffolded `.github/` agent crew with `TODO` markers. | **Fill in** the TODOs during the lab. |
| [`demo-employee-portal/`](./demo-employee-portal) | Small .NET 9 Web API with intentional code smells. | **Review** it with the agents you just built. |

Supporting material:

- [`Session2_Custom_Agents.pptx`](./Session2_Custom_Agents.pptx) — slide deck used during the live walkthrough
- [`LAB_GUIDE.md`](./LAB_GUIDE.md) — line-by-line creation guide (reference, deep version)
- [`MY_RUNBOOK.md`](./MY_RUNBOOK.md) — instructor runbook (90-min version)
- [`MY_RUNBOOK_FAST.md`](./MY_RUNBOOK_FAST.md) — instructor runbook (60-min version)

---

## What you will build

A crew of **four artifacts** plus one orchestration prompt:

```
.github/
├── copilot-instructions.md       # project-wide rules (always active)
├── agents/
│   ├── code-reviewer.agent.md    # @code-reviewer — reads & searches only
│   └── doc-writer.agent.md       # @doc-writer   — reads, searches, edits
├── skills/
│   └── doc-generator/
│       ├── SKILL.md              # /doc-generator — reusable recipe
│       ├── config.yaml           # branding + sections to emit
│       └── templates/
│           └── project-docs.html # HTML skeleton with {{placeholders}}
└── prompts/
    └── generate-docs.prompt.md   # entry point that ties it all together
```

The end result: type a single command in Copilot Chat and get a branded HTML document that reflects your company's colors, logo, and sections — generated entirely from the source code.

---

## The mental model (4 layers)

```
 PROMPTS        — orchestrate the flow       ( user-triggered )
 AGENTS         — persona + tools             ( @named or auto-delegated )
 SKILLS         — reusable recipes            ( invoked via /command )
 INSTRUCTIONS   — always-on rules             ( inherited by everything above )
```

**Key rule:** instructions flow upward. Every agent and skill automatically inherits the project-level rules in `copilot-instructions.md`.

**Rule of thumb:** *Agent = who. Skill = how.* The agent decides what to do; the skill knows how to do it.

---

## Prerequisites

| Tool | Why |
|------|-----|
| VS Code (latest) | Editor |
| GitHub Copilot extension, **Business** license or higher | Custom agents & skills are a Business feature |
| .NET 9 SDK | To build and optionally run `demo-employee-portal` |
| Git | Cloning |

---

## Quick start

```bash
git clone <this-repo-url> session2-lab
cd session2-lab/session2-custom-agents-crew

# 1. Open the lab folders in VS Code
code .

# 2. Verify Copilot is active (bottom-right corner of VS Code)

# 3. (Optional) Build the demo portal so C# intellisense works
cd demo-employee-portal
dotnet restore
dotnet build
cd ..
```

You are ready. During the workshop you will edit files inside `lab-starter/.github/` and test them against `demo-employee-portal/`.

---

## Workshop flow (60 min)

| Block | Time | What happens |
|-------|------|--------------|
| **0** — Intro | 0:00–0:10 | Speech + the pyramid (slides only) |
| **A** — First agent | 0:10–0:25 | Fill `copilot-instructions.md` + `code-reviewer.agent.md`, test on a controller |
| **B** — Doc generator | 0:25–0:55 | Pick a palette in `config.yaml`, fill `SKILL.md`, `doc-writer.agent.md`, `generate-docs.prompt.md`, **run the pipeline** |
| **C** — Demo & Q&A | 0:55–1:00 | Open the generated HTML, discuss |

---

## Files you will create (in order)

1. **`lab-starter/.github/copilot-instructions.md`** — project-wide coding rules
2. **`lab-starter/.github/agents/code-reviewer.agent.md`** — read-only reviewer
3. **`lab-starter/.github/skills/doc-generator/config.yaml`** — pick company name + 1 of 3 palettes
4. **`lab-starter/.github/skills/doc-generator/SKILL.md`** — step-by-step generation recipe
5. **`lab-starter/.github/agents/doc-writer.agent.md`** — persona that drives the skill
6. **`lab-starter/.github/prompts/generate-docs.prompt.md`** — orchestration entry point

The HTML template ([`templates/project-docs.html`](./lab-starter/.github/skills/doc-generator/templates/project-docs.html)) is already complete — don't edit it.

---

## How to test your agents

Open Copilot Chat inside VS Code with the `lab-starter/` folder as workspace root.

### Test the code-reviewer

```
@code-reviewer Review ../demo-employee-portal/Controllers/LeaveRequestsController.cs
```

A well-written reviewer should surface findings like:
- side effects in `GET` endpoints
- deeply nested validation chains
- `ToList().Where(...)` anti-patterns
- `Thread.Sleep` inside a request
- missing authorization / access-control checks
- exception messages leaked in responses

> The `demo-employee-portal/` codebase has **~25 planted issues** across three severity levels (security, performance, quality). If the reviewer misses something you expected, that is a signal to refine its checklist — that iteration is the point.

### Test the doc-writer pipeline

```
/generate-docs
```

(or attach `generate-docs.prompt.md` via the 📎 button and send)

Open the output:

```bash
start demo-employee-portal/docs/project-docs.html   # Windows
open  demo-employee-portal/docs/project-docs.html   # macOS
```

You should see: branded header, table of contents, API reference with real endpoints extracted from controllers, data model with real entities.

---

## The demo target: what's inside

[`demo-employee-portal/`](./demo-employee-portal) is a minimal Employee Management API (employees, leave requests, auth). It is **intentionally** filled with realistic legacy-style issues so the agents have something to find:

- SQL injection via `FromSqlRaw` with string interpolation
- MD5 password hashing + non-constant-time comparison (timing attack)
- Username enumeration in error messages
- Broken access control (missing ownership checks)
- Sensitive entity fields exposed directly (no DTO): `SalaryGross`, `TaxCode`, `BankIban`
- Side effects in `GET` endpoints (counter increment + save)
- `ToList().Where(...)` — filtering in memory instead of SQL
- N+1 query patterns
- `Thread.Sleep` inside controllers
- Nested if/else up to 5 levels deep
- God controllers with inline business logic
- Dead code: unused private methods, unreferenced fields, an `[Obsolete]` class still registered in DI
- Magic strings where an enum belongs
- `Console.WriteLine` instead of `ILogger<T>`
- Overly permissive CORS, missing HTTPS redirect

**Do not use this codebase as a reference for real projects.** It exists purely as a review target.

---

## Troubleshooting

| Problem | Fix |
|---------|-----|
| Copilot doesn't see your new agent | Reload the VS Code window (`Ctrl+Shift+P` → *Reload Window*) |
| `@doc-writer` isn't auto-delegated | Invoke it explicitly with `@doc-writer …` or sharpen the `description:` field in its frontmatter |
| Skill not found | Check the path is exactly `.github/skills/doc-generator/SKILL.md` (case-sensitive on macOS/Linux) |
| Generated HTML still contains `{{placeholder}}` markers | Re-run with an explicit instruction: *"Replace ALL placeholders before writing the file."* |
| `dotnet build` fails on the demo portal | Ensure .NET 9 SDK is installed: `dotnet --list-sdks` |

---

## Going further

The pattern you learned today is reusable. Same structure, different domain:

| Swap the skill for… | …and you have |
|---------------------|---------------|
| `/changelog-generator` | Automated release notes from commits |
| `/test-writer` | xUnit test scaffolding from controllers |
| `/migration-planner` | Upgrade plans for framework bumps |
| `/arch-reviewer` | Architectural audits against your diagrams |

**Homework:** clone your own repo, drop this `.github/` structure in, adjust the instructions to your stack, and re-run. Bring the output to the next session.

---

## License

Internal training material. Do not redistribute the `demo-employee-portal/` codebase outside of this lab.
