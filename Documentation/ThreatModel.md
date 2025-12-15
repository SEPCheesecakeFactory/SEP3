# Threat Model: Learnify

**Date:** 2025-12-15
**Version:** [1.3]

---

## 1. System Description

Learnify is a distributed e-learning platform that is designed to provide users with access to a wide range of educational resources and courses. The system consists of multiple components, including a database, data server, logic server, client web server, and more. The system is built for people seeking education, sharing knowledge, and administrators managing the platform.

## 2. Security Objectives

The security goals for this system based on the CIA triad (TODO: Source).

* **Confidentiality:** Protect user passwords and personal data from unauthorized disclosure.
* **Integrity:** Ensuring that data can not be altered or tampered with by unauthorized parties.
* **Availability:** Ensure the system remains accessible during high traffic or denial-of-service attempts.
* **Accountability:** Actions must be uniquely traceable to a specific entity.
* **Authenticity:** Verify that data inputs and users are genuine.

---

## 3. Adversary Model

Who the attackers are and where they are attacking from based on EIOO (TODO: Source).

### 3.1 Who is attacking?

* **External:** Hackers, Competitors
* **Internal:** Administrators, Teachers, Project Members

### 3.2 Where is the attack occurring?

* **Network:** Interception of traffic, man-in-the-middle attacks
* **Online:** Attacks on the database, data server, logic server, or client web server
* **Offline:** Physical access to servers, social engineering attacks, stealing data saved on local machines (e.g., JWT tokens, cached passwords)

---

## 4. Threat Categorization

Potential threats based on the attacker's goal based on STRIDE (TODO: Source).

| Category | Definition | Potential Threats to Learnify |
| :--- | :--- | :--- |
| **S**poofing Identity | Impersonating another person or system. | Attacker uses stolen credentials to log in as another user (most importantly as an administrator or teacher) |
| **T**ampering | Modifying data without detection. | Modifying database records directly or through the dataserver |
| **R**epudiation | Denying having performed an action. | A teacher claims they did not submit a draft, or edit course content. Admin claims they did not alter the user data (roles) |
| **I**nformation Disclosure | Accessing restricted data. | Leaking user personal information, course content that should be restricted |
| **D**enial of Service | Denying access to valid users. | Flooding the server with requests to crash it, e.g. bug in the client app that could cause a recursive call to the rest api or intentional attack |
| **E**levation of Privilege | Gaining higher rights than authorized. | A normal user exploits a bug to gain administrator privileges. |

---

## 5. Attack Surfaces and Vectors

The means and entry points used by attackers.

### 5.1 Attack Surfaces

* **Network:** Open ports, Wi-Fi, Firewalls, Intranet, VPNs
* **Software:** API endpoints, Database interfaces, IDEs and browsers
* **Human:** Social engineering, Phishing, Human error, Insider threats, Physical access, Lost devices

### 5.2 Attack Means

* **Passive Attacks:**
    * *Eavesdropping:* Monitoring transmissions for sensitive data.
    * *Traffic Analysis:* Observing patterns/frequency of communication.  
    * *Shoulder Surfing:* Observing user input directly (e.g., passwords).
    * *Dumpster Diving:* Searching through physical trash for sensitive information.
    * *Wiretapping:* Intercepting communication lines to capture data. (cables, etc.)

* **Active Attacks:**
    * *Masquerade:* Pretending to be a different entity.
    * *Replay:* Resending old messages to produce unauthorized effects.
    * *Modification:* Altering portions of a legitimate message.
    * *Denial of Service:* Preventing normal use of communications facilities.
    * *Release of Information:* Exposing sensitive data to unauthorized parties.
    * *Injection Attacks:* Inserting malicious code into input fields (e.g., SQL Injection, XSS).

### 5.3 Attack Tree (Optional)

The hierarchical paths of attacks, starting with the goal (Root) and branching into methods (Leaves)

* **Root Goal:** [e.g., Compromise User Bank Account]
    * **Branch 1:** [Compromise Credentials] -> [Phishing] OR [Brute Force]
    * **Branch 2:** [Injection of Commands] -> [SQL Injection]

---

## 6. Vulnerability Analysis (TPM)

Potential failures based on their source:

* **Threat Model:** [Are there threats we ignored? e.g., assuming internal network is safe.]
* **Policy:** [Does the policy allow unsafe actions? e.g., allowing weak passwords.]
* **Mechanism:** [Can the security mechanism be bypassed? e.g., software bug in the login code.]

---

## 7. Risk Assessment & Mitigation

| Threat ID | Threat Description | Likelihood | Impact | Risk Level | Mitigation Strategy |
| :--- | :--- | :--- | :--- | :--- | :--- |
| T-01 | SQL Injection on Login | High | High | **Critical** | Implement Input Validation and Parameterized Queries. |
| T-02 | Admin Password Phishing | Medium | High | **High** | Implement MFA for all admin accounts. |
| T-03 | DDoS Attack | Low | Medium | **Medium** | Configure Firewall rate limiting. |