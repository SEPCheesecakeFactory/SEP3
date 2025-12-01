DROP SCHEMA IF EXISTS learn_db CASCADE;
CREATE SCHEMA learn_db;
SET SCHEMA 'learn_db';

CREATE TABLE Language
(
    code VARCHAR(3) PRIMARY KEY,
    name VARCHAR(20) UNIQUE
);

CREATE TABLE CourseCategory
(
    id          SERIAL PRIMARY KEY,
    name        VARCHAR(50),
    description VARCHAR(200)
);

CREATE TABLE Course
(
    id          SERIAL PRIMARY KEY,
    language    VARCHAR(3) REFERENCES Language (code),
    title       VARCHAR(50),
    description VARCHAR(300),
    category    INT REFERENCES CourseCategory (id)
);

CREATE TABLE SystemUser
(
    id SERIAL PRIMARY KEY,
    username VARCHAR(20),
    password_hash VARCHAR(512)
);

CREATE TABLE Role
(
    role VARCHAR(7) PRIMARY KEY NOT NULL CHECK (role IN ('learner', 'teacher', 'admin'))
);

CREATE TABLE SystemUserRole
(
    systemUserId int REFERENCES SystemUser(id),
    role varchar(7) REFERENCES Role(role),
    PRIMARY KEY (systemUserId, role)
);
CREATE TABLE LearningStepType
(
    id   SERIAL PRIMARY KEY,
    name VARCHAR(50)
);

CREATE TABLE LearningStep
(
    step_order  INT,
    course_id   INT REFERENCES Course (id),
    PRIMARY KEY (step_order, course_id),
    step_type   INT REFERENCES LearningStepType (id),
    content     TEXT
);

-- Insert into LearningStepType
INSERT INTO LearningStepType (name) VALUES
('Text'),
('Video');

-- Insert into Language
INSERT INTO Language (code, name) VALUES
('ENG', 'English');

-- Insert into CourseCategory
INSERT INTO CourseCategory (name, description) VALUES
('History', 'Courses related to historical events and periods.');

-- Insert into Course
INSERT INTO Course (language, title, description, category) VALUES
('ENG', 'The Roman Empire', 'An in-depth look at the events of the Roman Empire.', 1);


INSERT INTO Course (language, title, description, category) VALUES ('ENG', 'New Course #2', 'Brand new course #2', 1);

-- Insert into SystemUser
INSERT INTO SystemUser (username, password_hash)
VALUES ('admin_user', '123'),
('alice_smith', '123'),
('jane_doe', '123');

-- Insert into Role
INSERT INTO Role (role) VALUES 
('admin'), 
('learner'), 
('teacher');

-- Insert into SystemUserRole
INSERT INTO SystemUserRole (systemUserId, role) VALUES 
-- admin_user (ID 1) gets 'admin' and 'learner'
(1, 'admin'),
(1, 'learner'),
-- alice (ID 2) gets 'learner'
(2, 'learner'),
-- jane (ID 3) gets 'teacher' and 'learner'
(3, 'teacher'),
(3, 'learner');
-- Insert into LearningStep
INSERT INTO LearningStep (step_order, course_id, step_type, content) VALUES
(1, 1, 1, 'Introduction to the Roman Empire.'),
(2, 1, 2, 'https://www.youtube.com/watch?v=46ZXl-V4qwY'),
(3, 1, 1, 'The fall of the Roman Empire and its legacy.');
 
-- Questions --
UPDATE LearningStep
SET step_order = 5
WHERE course_id = 1 AND step_order = 3;
UPDATE LearningStep
SET step_order = 3
WHERE course_id = 1 AND step_order = 2;

INSERT INTO LearningStepType (id, name)
VALUES (3, 'Question');

INSERT INTO LearningStep (step_order, course_id, step_type, content)
VALUES (
    2,
    1,
    3,
    'QUESTION_MC|Who was the first Emperor of the Roman Empire?|Augustus|Julius Caesar|Nero|Tiberius'
);

INSERT INTO LearningStep (step_order, course_id, step_type, content)
VALUES (
    4, 1, 3,
    'QUESTION_FILL|The capital city of the Roman Empire was ___ .|Rome'
);
