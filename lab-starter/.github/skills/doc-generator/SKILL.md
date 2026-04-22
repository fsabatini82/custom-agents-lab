---
name: doc-generator
description: >
  TODO: Describe what this skill does. Copilot uses this to decide
  when to invoke the skill automatically.
allowed-tools:
  # TODO: This skill needs to read files AND write the output.
  # Which tools are required?
  - read
---

# Doc Generator Skill

TODO: Write a one-line summary of what this skill produces.

## Step 1: Read Configuration

Read `.github/skills/doc-generator/config.yaml` for all settings:
- TODO: What branding settings?
- TODO: What document settings?
- TODO: What sections list?
- TODO: What output settings?

## Step 2: Read Template

Read `.github/skills/doc-generator/templates/project-docs.html`.

The template uses `{{placeholder}}` markers:

| Placeholder | Source |
|-------------|--------|
| `{{company}}` | config -> branding.company |
| TODO: Add remaining placeholders |

## Step 3: Generate Content per Section

For each section in `config.yaml -> sections`, generate HTML content:

### project-overview
TODO: What files to read? What to extract? What format?

### architecture
TODO: What folders to scan? What to describe?

### api-reference
TODO: How to find controllers? What to extract per endpoint?

### data-model
TODO: Where are models? What properties to document?

### coding-conventions
TODO: Where to find conventions? How to format?

### getting-started
TODO: What setup steps to document?

## Step 4: Build Table of Contents

TODO: How should the TOC be generated?

## Step 5: Assemble and Write

TODO: How to replace placeholders, inject content, write output?

## Quality Rules
- Every section must have REAL content from the project
- No leftover {{placeholder}} markers in the output
- TODO: Add more quality rules
