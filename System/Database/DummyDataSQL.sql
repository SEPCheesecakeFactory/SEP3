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