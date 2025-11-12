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

-- Insert into LearningStep
INSERT INTO LearningStep (step_order, course_id, step_type, content) VALUES
(1, 1, 1, 'Introduction to the Roman Empire.'),
(2, 1, 2, 'https://www.youtube.com/watch?v=46ZXl-V4qwY'),
(3, 1, 1, 'The fall of the Roman Empire and its legacy.');