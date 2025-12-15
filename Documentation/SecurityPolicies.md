# Security Policy for Learnify

**Semester Project 3 - SEP3**
**Date:** 2025-12-15
**Version:** [1.4]

---

## Introduction
This document outlines the security policy for **Learnify** within **SEP3**. The system is an e-learning platform and requires appropriate security measures to protect data, ensure proper access control, and maintain system availability.

## Purpose
The purpose of this policy is to define the necessary security measures to protect **Learnify** from unauthorized access, data breaches, disruptions, or misuse. These measures ensure the **confidentiality, integrity, and availability** of the system.

## Scope
This policy applies to all users, administrators, contractors, and third-party service providers who access or interact with **Learnify**.

## Roles and Responsibilities

* **Product Owner:** Responsible for ensuring the system operates securely and this policy is implemented to the extent feasible.
* **System Administrators:** Responsible for maintenance, patching, account management, and monitoring.
* **Users:** Responsible for adhering to the policy and reporting suspicious activity.

---

## Access Control

### User Authentication

All users must authenticate using a unique username and password.

* **Requirement:**
    * Strong passwords (min 8 chars) required. MFA recommended for admins.
* **Rotation:** Passwords should be changed every 180 days.

### Role-Based Access Control (RBAC)

Access permissions are assigned based on the user’s roles.

* **Requirement:**
    * Simple RBAC with regular access reviews.

### Account Management

* **Inactive Accounts:**

    * Accounts will not be disabled automatically but will be a subject to periodic review.
    * Roles removed immediately after administrator or teacher exit.

---

## Data Protection

### Data Classification

Data is classified into categories - Public, Internal, and Sensitive.

* **Requirement:**
    * Internal data must be protected; Sensitive data requires encryption.

### Data Encryption

* **Requirement:**

    * Encrypt in transit (HTTPS); rest encryption optional.

### Data Backup

* **Requirement:**

    * Weekly backups. Retention: 30 days.

### Data Retention

* **Requirement:**

    * Retain and dispose of data per legal requirements.

---

## System Security

### Patch Management

* **Critical Patches:**
    * Apply within 24-48 hours.

### Firewall and Network Security

* **Requirement:**
    * Firewall with IDS/IPS and regular monitoring.

### Antivirus and Malware Protection

* **Requirement:**
    * Automatically updated antivirus and anti-malware on all systems.

### Physical Security

* **Requirement:**
    * Secure server locations with restricted access. (outsourced to cloud provider)

---

## Monitoring and Logging

### User Activity Logging

* **Requirement:**
    * Basic logging. Retain 90 days.

### System Monitoring

* **Requirement:**
    * Monthly review of basic metrics.

### Audit

* **Requirement:**
    * Annual security audit or after major changes.

---

## Incident Response

### Reporting

All users must report suspected incidents within 24 hours.

### Incident Response Plan

* **Requirement:**
    * Basic procedures (identify, notify, restore).

### Data Breach Notification

* **Requirement:**
    * Internal notification; external as legally required.

---

## Disaster Recovery and Business Continuity

### Disaster Recovery Plan (DRP)

* **Target Restoration:**
    * Within 14 days unless critical or legally mandated.

### Business Continuity

* **Requirement:**
    * Redundancy to minimize downtime.

---

## Compliance and Training

### Regulatory Compliance

* **Requirement:**
    * Basic legal compliance based on the system's operational regions.

### User Training

* **Frequency:**
    * Onboarding as needed; periodic optional refreshers.

---

## Glossary

* **MFA:** Multi-Factor Authentication
* **RBAC:** Role-Based Access Control
* **IDS/IPS:** Intrusion Detection/Prevention System
* **DRP:** Disaster Recovery Plan
* **HTTPS:** Hypertext Transfer Protocol Secure

---

## Policy Review and Approval

This policy will be reviewed **quarterly** or upon significant system changes.

**Last Reviewed:** 2025-12-15
**Approved By:** Eduard Fekete
**Next Review Date:** 2026-03-15