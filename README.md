# ClearHaul GitHub Pages Website

Static project website for ClearHaul, an open-source direct freight marketplace.

## Preview locally

From this folder, run:

```powershell
python -m http.server 8080
```

Then open `http://localhost:8080`.

## Publish with GitHub Pages

1. Create a GitHub repository or copy these files into the root of the ClearHaul repository.
2. Commit and push the files to the `main` branch.
3. Open the repository on GitHub.
4. Go to **Settings → Pages**.
5. Under **Build and deployment**, choose **GitHub Actions**.
6. The included workflow publishes the site after each push to `main`.

If the website is stored in a `/docs` directory instead of the repository root, change `path: .` to `path: ./docs` in `.github/workflows/pages.yml`.

## Project structure

```text
index.html
styles.css
script.js
assets/favicon.svg
.github/workflows/pages.yml
```

The site uses no external JavaScript packages, fonts, analytics or trackers.
