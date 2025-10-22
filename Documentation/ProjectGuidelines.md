# Purpose

The project guidelines outline the standards and best practices for contributing to and maintaining the project. They ensure consistency, quality, and collaboration among all team members. The main purpose is to align everyone on the same page regarding coding standards, documentation, version control, and communication methods.

# Scope

These guidelines apply to all work related to the project, including but not limited to code contributions, documentation updates, communication methods, and project management practices. They are intended for all team members, and potentially other stakeholders involved in the project.

# Process Definition

**Kanban / Task Board**: A space where each task has one of the predefined states. The states dictate what should happen to the given task.

**Task**: A single unit of work that needs to be completed.

**Task States**:

- **To Do**: Tasks that are planned but not yet started.

- **In Progress**: Tasks that are currently being worked on.

- **In Review**: Tasks that have been completed and are awaiting review or approval. 

- **Done**: Tasks that have been completed and approved. 

**User Story**: A description of a feature from an end-user perspective, typically following the format: "As a [type of user], I want [an action] so that [a benefit]."

**Backlog**: A prioritized list of tasks with the To Do state.

**Concepts**: A collection of abstracted task ideas that can be broken down into proper tasks later.

**Iteration**: A time-boxed period with an agreed state of the project to be achieved by the end of that period.

## Task Lifecycle

The process is driven by user stories. The user stories are either broken down into tasks directly or first collected as concepts and later broken down into tasks. The tasks are then prioritized in the backlog.

The tasks are owner-less by default. Iteration planning and other meetings can be used to assign ownership or responsibility for tasks.

![Task Lifecycle](./Figures/TaskLifecycle.png)

## Roles

- **Product Owner**: Responsible for defining the project vision, managing the backlog, and communicating everything with the Development Team.
- **Development Team**: Everyone involved in the development process.

## Testing and Refactoring

Not all code has to have a 100% unit test coverage. However, a functionality can't just be assumed without proper testing and should be verified via the reviewing processes. 

The main purpose for utilizing automated testing is to promote change and refactoring. If proper tests are in place, the code can be refactored and improved without the fear of breaking existing functionality.

Refactoring or increasing the test coverage can be done outside of the main feature development process, and should not be bound by bureaucratic processes to decrease the friction of improving the codebase.

## Reviewing

All tasks should be reviewed. If not necessary, the whole development team does not have to be involved in a review. The reviewer should either apply their own knowledge and expertise or consult with another party to ensure the quality of the work being reviewed.

