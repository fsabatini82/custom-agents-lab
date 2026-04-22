---
name: code-reviewer
description: >
  TODO: Write a description that tells Copilot WHEN to delegate to this agent.
  Think: what tasks should trigger this agent automatically?
tools:
  # TODO: Which tools does a code reviewer need?
  # Options: read, search, edit, execute, agent
  # Hint: A reviewer should ANALYZE, not MODIFY
  - read
disable-model-invocation: false
---

# Code Reviewer Agent

TODO: Write the system prompt. Who is this agent? What is their expertise?

## Review Checklist

### Security (CRITICAL)
- TODO: What security issues should it look for?

### Architecture (HIGH)
- TODO: What architectural problems should it flag?

### Code Quality (MEDIUM)
- TODO: What code quality issues should it detect?

## Output Format

TODO: Define how findings should be reported.
Hint: severity, file, description, fix suggestion
