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