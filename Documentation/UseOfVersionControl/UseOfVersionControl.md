# Use of Version Control

This document outlines how version control has been and is being utilized in this project.

For version control, we are using Git, a distributed version control system that allows multiple developers to work on the same codebase simultaneously while keeping track of changes made over time. For hosting our Git repository, we are using GitHub, a web-based platform that provides a collaborative environment for managing Git repositories. We are using several Git clients, including Git Bash, GitHub Desktop, VS Code plugin, IntelliJ IDEA plugin, LazyGit, and more.

## Git Workflow

We are using Git as our version control system. Rules for our workflow are specified in the [ProjectGuidelines.pdf](../Final/ProjectGuidelines.pdf) file. Citing them:

"The project uses Git for version control. All features and bug fixes should be developed in separate branches,
which are then merged into the main branch. The main branch can be considered always deployable.
For simplicity, the branches can either rebase from main before merging or use merge commits. Squashing
commits is only encouraged when the feature development has many trivial commits that do not add value to
the project history.

The process of merging should be primarily done by the person reviewing as the main branch should only
consist of reviewed and approved code. It is not the reviewer’s responsibility to resolve the merge conflicts if
any arise but minor conflicts should be resolved by the reviewer when possible to speed up the process.
For commit messages, guidelines such as Commit Principles (Fekete, 2025) can be followed."

## Repository

The main repository for this project is hosted on GitHub at the following URL:

[https://github.com/SEPCheesecakeFactory/SEP3](https://github.com/SEPCheesecakeFactory/SEP3)

## Branching Strategy

We have one main branch where the stable code resides. All new features and bug fixes are developed in separate branches, which are then merged into the main branch after thorough code review and testing. All such branches are labeled accordingly, e.g., `feature/feature-name`, `bugfix/bug-description`.

## Statistics

As of the latest update, the repository contains:

- Total commits: 144
- Total branches: 6

\newpage

To visualize commit activity over time, refer to the following plot generated from the repository data:

![Repository Commit History](UseOfVersionControl/image.png)

## Documentation

![Git Tree/Log](UseOfVersionControl/image-1.png){ width=250px }

![Git Tree/Log, deeper](UseOfVersionControl/image-2.png){ width=250px }
