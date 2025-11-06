# Contributing Guide

How to set up, code, test, review, and release so contributions meet our Definition of Done.

## Code of Conduct

- Team members must be respectful to one another.
- Team members must speak in a professional objective manner.

Conflict resolution:  
We will first discuss risks/conflicts with the 1-2 team members it involves, then if the conflict is unresolved, discuss it with the entire team. Most conflicts should be resolved by this point. if we are still unable to resolve the issue, we escalate to Alex Ulbrich (Project Partner) with commit history, communication logs, on top of what we have agreed to work on as a team as evidence.  

## Getting Started

Prerequisites:  
Unity Editor (latest Ver.) with student license  
Git  

Optional code editors:  
Visual Studio Code  
Visual Studio 2022  
or others  

Setup:  
https://unity.com/download  

Running app locally:  
- Open project in Unity Editor  
- Run project through in engine "Play" button/command  

Release builds will be compiled to an executable file  

## Branching & Workflow

Default Branch: 
Main  

Testing branches:  
 /Main/test-[testname]  

New Feature:  
/feature/[featurename]  

Merging to main:  
Pushing to main should only be done after atleast 1 peer review  

## Issues & Planning

Explain how to file issues, required templates/labels, estimation, and triage/assignment practices.  

Issues/bug reports should be added to:  
https://github.com/Rats-Incorporated/2D-Game-Dev/issues  

New Features should added to the Project Plan Board:  
https://github.com/users/neales44/projects/3  


## Commit Messages

All commits should include context of the update.  

i.e. Hitbox Update  

## Code Style, Linting & Formatting

(Unity Project) Currently no Linting method implimented.  

## Testing

Define required test types, how to run tests, expected coverage thresholds, and when new/updated tests are mandatory.  

Testing should be done through Unity Editor by using Unity Play button/command.  

Tests should check for:  
Fully funciton/runnable project  
Check that previous functionality still works  
Check new feature being added  

## Pull Requests & Reviews

Outline PR requirements (template, checklist, size limits), reviewer expectations, approval rules, and required status checks.  

Review Guidelines:  
- Atleast 1 team member must review merges to main  
- Review must be done by running feature/build in Unity editor  

## CI/CD

We are using GameCI Unity CI:
https://github.com/Rats-Incorporated/2D-Game-Dev/blob/main/.github/workflows/unityci.yml

Documentation found here:
https://game.ci/docs/github/getting-started#simple-example

## Security & Secrets

Github should be set to private  

Do not commit API keys to GitHub  
Reporting vulnerabilities:  
Discuss with team members or project partner via email, discord message  

## Documentation Expectations

If any 3rd party code/assets are used, specific references must be made in README file as well as stating copyright status.  

update CHANGELOG.md for end user changes  


## Release Process

Releases must be labeled in the following versioning scheme:  
v.1.1.0  

CHANGELOG.md must be updated before release  

## Support & Contact

Maintainers:  
Roxlynn Beecher - beecheco@oregonstate.edu  
Austin Christian - chrisaus@oregonstate.edu  
Ethan Ferrante - ferrante@oregonstate.edu  
Michael Hand - handm@oregonstate.edu  
Samuel Neale - neales@oregonstate.edu  

Contact Channels:  
Email or Discord(#game-dev)  

Expected response window:  
24 Hours  
