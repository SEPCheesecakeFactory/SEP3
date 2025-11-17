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

CREATE TABLE User
(
    id SERIAL PRIMARY KEY,
    username VARCHAR(20),
    password_hash VARCHAR(512),
    role VARCHAR(7) NOT NULL CHECK (role IN ('learner', 'teacher', 'admin'))
);

-- Insert into Language
INSERT INTO Language (code, name) VALUES
('ENG', 'English'),
('SPA', 'Spanish'),
('GER', 'German'),
('FRA', 'French'),
('JPN', 'Japanese');

-- Insert into CourseCategory
INSERT INTO CourseCategory (name, description) VALUES
('Programming', 'Courses related to software development and coding.'),
('Design', 'Courses on graphic design, UX/UI, and creative design principles.'),
('Language Learning', 'Courses to help learn new spoken languages.'),
('Business', 'Courses about management, marketing, and entrepreneurship.'),
('Data Science', 'Courses about statistics, data analysis, and AI.');

-- Insert into Course
INSERT INTO Course (language, title, description, category) VALUES
('ENG', 'Java Basics', 'Learn the fundamentals of Java programming.', 1),
('ENG', 'Web Design Fundamentals', 'Introduction to HTML, CSS, and design layout principles.', 2),
('SPA', 'Español para Principiantes', 'Learn basic Spanish vocabulary and grammar.', 3),
('GER', 'Business Management 101', 'Introduction to core business concepts and management skills.', 4),
('FRA', 'Data Science with Python', 'Learn how to analyze data using Python libraries.', 5),
('JPN', 'Advanced Java Concepts', 'Deep dive into Java streams, concurrency, and best practices.', 1),
('ENG', 'Digital Marketing', 'Master SEO, social media, and online advertising.', 4),
('SPA', 'Diseño UX/UI', 'Aprende a diseñar interfaces centradas en el usuario.', 2),
('FRA', 'Artificial Intelligence Basics', 'Understand the basics of AI and machine learning.', 5),
('ENG', 'Effective Communication', 'Improve your communication and presentation skills.', 4),
('ENG', 'New Course', 'Brand new course.', 4);


INSERT INTO Course (language, title, description, category) VALUES ('ENG', 'New Course #2', 'Brand new course #2', 4);

-- Insert into User
INSERT INTO User (username, password_hash, role)
VALUES ('admin_user', '123', 'admin');
VALUES ('alice_smith', '123', 'learner');
VALUES ('jane_doe', '123', 'teacher');