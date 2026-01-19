#!/usr/bin/env bash
set -euo pipefail
cd /Users/igorsedykh/Learn/XTCheck
if [ -d .git ]; then echo "[INFO] .git exists"; else git init && echo "[INFO] git initialized"; fi
# ensure branch main exists
if ! git rev-parse --verify main >/dev/null 2>&1; then
  git checkout -b main || true
fi
# add and commit
git add -A
if git commit -m "Initial commit"; then
  echo "[INFO] committed"
else
  echo "[INFO] nothing to commit"
fi
# attempt to create and push via gh
if command -v gh >/dev/null 2>&1; then
  echo "[INFO] gh CLI found — attempting to create remote repo"
  gh repo create --public --source=. --remote=origin --push --confirm || echo "[WARN] gh repo create failed"
else
  echo "[WARN] gh CLI not installed or not authenticated"
fi
# show remotes and repo view
git remote -v || true
if command -v gh >/dev/null 2>&1; then
  gh repo view --json url -q .url 2>/dev/null || true
fi
