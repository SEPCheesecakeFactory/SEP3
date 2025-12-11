DROP SCHEMA IF EXISTS learn_db CASCADE;

CREATE SCHEMA learn_db;

SET SCHEMA 'learn_db';

-- 1. Independent Tables
CREATE TABLE Language (
    code VARCHAR(3) PRIMARY KEY,
    name VARCHAR(20) UNIQUE
);

CREATE TABLE CourseCategory (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50),
    description VARCHAR(200)
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

CREATE TABLE LearningStepType (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50)
);

-- 2. Dependent Tables (Level 1)

CREATE TABLE SystemUserRole (
    systemUserId int REFERENCES SystemUser (id),
    role varchar(7) REFERENCES Role (role),
    PRIMARY KEY (systemUserId, role)
);

CREATE TABLE Course (
    id SERIAL PRIMARY KEY,
    language VARCHAR(3) REFERENCES Language (code),
    title VARCHAR(50),
    description VARCHAR(300),
    category INT REFERENCES CourseCategory (id),
    -- New fields per GR Diagram
    author_id INT REFERENCES SystemUser (id),
    approved_by INT REFERENCES SystemUser (id)
);

-- 3. Dependent Tables (Level 2)

CREATE TABLE LearningStep (
    step_order INT,
    course_id INT REFERENCES Course (id),
    step_type INT REFERENCES LearningStepType (id),
    content TEXT,
    PRIMARY KEY (step_order, course_id)
);

CREATE TABLE user_course_progress (
    user_id INT REFERENCES SystemUser (id) ON DELETE CASCADE,
    course_id INT REFERENCES Course (id) ON DELETE CASCADE,
    current_step INT DEFAULT 1,
    PRIMARY Key (user_id, course_id)
);

-- =============================================
-- SEED DATA INSERTION
-- =============================================

-- 1. Reference Data
INSERT INTO
    LearningStepType (name)
VALUES ('Text'),
    ('Video'),
    ('QuestionMC'),
    ('QuestionFILL');

INSERT INTO Language (code, name) VALUES ('ENG', 'English');

INSERT INTO
    CourseCategory (name, description)
VALUES (
        'History',
        'Courses related to historical events and periods.'
    ),
    (
        'Software Engineering',
        'Courses focused on software development practices and tools.'
    ),
    ('default', '');

INSERT INTO
    Role (role)
VALUES ('admin'),
    ('learner'),
    ('teacher');

-- 2. Users (Must exist before courses for author_id)
-- insert admin with password $argon2i$v=19$m=16,t=2,p=1$am5PMUNzT1FSeTg2UXVaVA$FEnJk9CY+ATMEVtpIP91tQ
INSERT INTO
    SystemUser (username, password_hash)
VALUES (
        'superuserito',
        '$argon2i$v=19$m=16,t=2,p=1$am5PMUNzT1FSeTg2UXVaVA$FEnJk9CY+ATMEVtpIP91tQ'
    ),
    (
        'adminito',
        '$argon2i$v=19$m=16,t=2,p=1$am5PMUNzT1FSeTg2UXVaVA$FEnJk9CY+ATMEVtpIP91tQ'
    ),
    (
        'teacherito',
        '$argon2i$v=19$m=16,t=2,p=1$am5PMUNzT1FSeTg2UXVaVA$FEnJk9CY+ATMEVtpIP91tQ'
    ),
    (
        'userito',
        '$argon2i$v=19$m=16,t=2,p=1$am5PMUNzT1FSeTg2UXVaVA$FEnJk9CY+ATMEVtpIP91tQ'
    );

INSERT INTO
    SystemUserRole (systemUserId, role)
VALUES (1, 'learner'),
    (1, 'admin'),
    (1, 'teacher'),
    (2, 'learner'),
    (2, 'admin'),
    (3, 'learner'),
    (3, 'teacher'),
    (4, 'learner');

-- 3. Courses (Updated to include author_id and approved_by)
INSERT INTO
    Course (
        language,
        title,
        description,
        category,
        author_id,
        approved_by
    )
VALUES (
        'ENG',
        'The Roman Empire',
        'An in-depth look at the events of the Roman Empire.',
        1,
        1,
        1
    ),
    (
        'ENG',
        'Git & GitHub',
        'Basics of Git version control and GitHub collaboration.',
        2,
        1,
        1
    );

-- 4. Learning Steps
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
        'Git Architecture: DAG and Three-Tree Model...'
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
        'Branching Strategy: Merge vs. Rebase...'
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
        'Remotes and GitHub...'
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
        'What is the destructive consequence of using "git push --force" on a shared branch?|It overwrites remote history...|It automatically merges conflicts.|It duplicates the repository.|It locks the branch.'
    ),
    (
        10,
        2,
        4,
        'To copy a remote repository to your local machine for the first time, use: git ___ .|clone'
    );