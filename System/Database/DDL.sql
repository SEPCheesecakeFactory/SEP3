DROP SCHEMA IF EXISTS learn_db CASCADE;

CREATE SCHEMA learn_db;

SET SCHEMA 'learn_db';

CREATE TABLE Language (
    code VARCHAR(3) PRIMARY KEY,
    name VARCHAR(20) UNIQUE
);

CREATE TABLE CourseCategory (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50),
    description VARCHAR(200)
);

CREATE TABLE Course (
    id SERIAL PRIMARY KEY,
    language VARCHAR(3) REFERENCES Language (code),
    title VARCHAR(50),
    description VARCHAR(300),
    category INT REFERENCES CourseCategory (id)
);

CREATE TABLE SystemUser (
    id SERIAL PRIMARY KEY,
    username VARCHAR(20),
    password_hash VARCHAR(512)
);

CREATE TABLE Role (
    role VARCHAR(7) PRIMARY KEY NOT NULL CHECK (
        role IN ('learner', 'teacher', 'admin')
    )
);

CREATE TABLE SystemUserRole (
    systemUserId int REFERENCES SystemUser (id),
    role varchar(7) REFERENCES Role (role),
    PRIMARY KEY (systemUserId, role)
);

CREATE TABLE LearningStepType (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50)
);

CREATE TABLE LearningStep (
    step_order INT,
    course_id INT REFERENCES Course (id),
    PRIMARY KEY (step_order, course_id),
    step_type INT REFERENCES LearningStepType (id),
    content TEXT
);

CREATE TABLE CourseDraft
(
    id serial PRIMARY KEY,
    language varchar(50),
    title varchar(50),
    description varchar(300),
    teacher_id int REFERENCES SystemUser(id),
    course_id int REFERENCES Course(id)
);

-- Insert into LearningStepType
INSERT INTO
    LearningStepType (name)
VALUES ('Text'),
    ('Video'),
    ('QuestionMC'),
    ('QuestionFILL');

-- Insert into Language
INSERT INTO Language (code, name) VALUES ('ENG', 'English');

-- Insert into CourseCategory
INSERT INTO
    CourseCategory (name, description)
VALUES (
        'History',
        'Courses related to historical events and periods.'
    ),
    (
        'Software Engineering',
        'Courses focused on software development practices and tools.'
    );

-- Insert into Course
INSERT INTO
    Course (
        language,
        title,
        description,
        category
    )
VALUES (
        'ENG',
        'The Roman Empire',
        'An in-depth look at the events of the Roman Empire.',
        1
    ),
    (
        'ENG',
        'Git & GitHub',
        'Basics of Git version control and GitHub collaboration.',
        2
    );

-- Insert into Role
INSERT INTO
    Role (role)
VALUES ('admin'),
    ('learner'),
    ('teacher');

-- Questions --
UPDATE LearningStep
SET
    step_order = 5
WHERE
    course_id = 1
    AND step_order = 3;

UPDATE LearningStep
SET
    step_order = 3
WHERE
    course_id = 1
    AND step_order = 2;

-- Insert into LearningStep
INSERT INTO
    LearningStep (
        step_order,
        course_id,
        step_type,
        content
    )
VALUES (
        1,
        1,
        1,
        'Introduction to the Roman Empire.'
    ),
    (
        2,
        1,
        2,
        'https://www.youtube.com/watch?v=46ZXl-V4qwY'
    ),
    (
        3,
        1,
        1,
        'The fall of the Roman Empire and its legacy.'
    ),
    (
        4,
        1,
        3,
        'Who was the first Emperor of the Roman Empire?|Augustus|Julius Caesar|Nero|Tiberius'
    ),
    (
        5,
        1,
        4,
        'The capital city of the Roman Empire was ___ .|Rome'
    );

INSERT INTO
    LearningStep (
        step_order,
        course_id,
        step_type,
        content
    )
VALUES (
        1,
        2,
        1,
        'Git Architecture: DAG and Three-Tree Model. Git is not just a backup system; it is a distributed system based on a Directed Acyclic Graph (DAG). You must understand the three states: Working Directory, Staging Area (Index), and the Repository (HEAD) to effectively manage snapshots.'
    ),
    (
        2,
        2,
        2,
        'https://www.youtube.com/watch?v=r8jQ9hVA2qs'
    ),
    (
        3,
        2,
        3,
        'Which command moves changes from the Working Directory to the Staging Area (Index)?|git add|git commit|git push|git init'
    ),
    (
        4,
        2,
        1,
        'Branching Strategy: Merge vs. Rebase. Branches are cheap pointers to commits. Merging preserves history topology, while rebasing rewrites history for linearity. Warning: Never rebase shared branches (public history) as it de-synchronizes collaborators.'
    ),
    (
        5,
        2,
        2,
        'https://www.youtube.com/watch?v=qI6yrLpu66s'
    ),
    (
        6,
        2,
        4,
        'To switch to an existing branch named "hotfix", the classic command is: git ___ hotfix.|checkout'
    ),
    (
        7,
        2,
        1,
        'Remotes and GitHub. "Origin" is simply the default alias for your remote URL. Pull Requests are not a Git command; they are a platform-specific (GitHub/GitLab) workflow for code review before merging.'
    ),
    (
        8,
        2,
        2,
        'https://www.youtube.com/watch?v=KhORHy58Q-s'
    ),
    (
        9,
        2,
        3,
        'What is the destructive consequence of using "git push --force" on a shared branch?|It overwrites remote history, potentially causing data loss for team members.|It automatically merges conflicts.|It duplicates the repository.|It locks the branch.'
    ),
    (
        10,
        2,
        4,
        'To copy a remote repository to your local machine for the first time, use: git ___ .|clone'
    );
