# Problem Domain

## Education problem

Education is one of the most important aspects of life in civilized societies (NAICU - NAICU - Improves Quality of Life, 2025). Humans need knowledge in order to function in today's world — from simple communication to career growth. Knowledge can improve quality of life in many ways (Burke, 2023). "Nowadays around 40% of the global population does not have access to proper education in a language they understand" (PTI, 2025).

Many countries and communities struggle with gender inequality in educational opportunities, which ultimately results in educational disparities and economic inequality. Men and women should be able to choose their educational paths without being constrained by gender norms or stereotypes (Gender Equality in a Changing World, 2025).

In addition to unequal access, gender inequality is also visible in the socialization and education of boys and girls, even when both have equal formal opportunities. Gender norms and stereotypes at early stages influence the types of skills children are motivated to learn and the careers they are likely to pursue. For example, international test scores show that around age ten, girls read more effectively than boys, while boys perform better in mathematics and science (OECD, 2022). These patterns reflect not only differences in ability but also the effects of socialization, classroom processes, and cultural expectations about "appropriate" fields of study for each sex.

These inequalities are also shaped by intersecting variables, including socioeconomic status, physical ability, and social status (OECD, 2020).

## Current Situation

Digital free-for-all educational platforms have been on the rise in recent years because they can provide knowledge to anyone with an internet connection and a device, thus minimizing educational inequalities (Duolingo, 2025).

However, many currently operating platforms focus on different aspects of education, such as language learning, coding, mathematics, or science (Singh et al., 2015). This leaves gaps and does not provide a consistent learning experience for users. Additionally, some platforms offer little free content, which can be a significant barrier for low-income users (World Bank Group, 2025).

Despite rapid advances in digital technologies, a major challenge persists: 2.6 billion people remain offline (Poggi, 2025). The digital divide is a major barrier to economic growth and sustainable development, with only 27% of the population in low-income countries having access to the internet, compared to 93% in high-income countries (Poggi, 2025).

## Stakeholders

The primary stakeholders in the field of education include both learners and knowledge providers worldwide. Learners can be individuals of all ages, backgrounds, and locations who seek to acquire new skills or knowledge. Knowledge providers can include educational institutions, teachers, tutors, and online platforms that offer educational content and resources. Other stakeholders may include governments, non-profit organizations, and businesses interested in promoting education and improving access to learning opportunities.

---

# Problem Statement

## Main problem:

How to provide easily accesible learning platform?

## Sub-questions:

- Is there any way to speed up the learning process?

- How to make the learning process efficient and user friendly?

- How to ensure that all genders can acquire proper knowledge?

- What design principles can be used to make digital learning app more convenient?

- How can accessibility features (e.g., screen readers, voice commands, adaptive interfaces) be integrated for users with disabilities?

- How can trust in the system be established and maintained?

- How can the correctness of knowledge be ensured?

- How to provide and maintain security within the app?

---

# Delimitation

The project focuses on helping individuals who cannot access formal education. This may be due to income reasons or geographic constraints. Men and women will have equal opportunities on the digital platform, since everyone should be able to pursue education as they wish. To ensure high-quality, accurate content, courses will be reviewed by experts and AI. 

---

# Choice of Methods

## Knowledge and Data Collection

To identify the educational and technical issues of digital learning platforms, we will:

- Review existing platforms (e.g., Coursera, Duolingo, Udemy) to ascertain their strengths and weaknesses.
- Review academic reports and papers on the digital divide and language learning strategies.
- Identify user needs from informal questionnaires and discussions within the project team.

## Analysis and Modelling

For distributed system planning and design, UML diagrams will be used to model system functionality and interactions.

- Architectural patterns such as client–server and REST-based architectures will be part of the design.
- Threat modelling will be conducted and potential security threats in authentication and data communication will be analysed.

## Design, Construction and Implementation

The system will be developed using standard software development methods:

- Agile methodology with short iterations to ensure continuous progress and responsiveness.
- UI/UX prototyping with Figma for wireframing and user flows prior to coding.
- Backend services implemented in Java/C# with RESTful web services.
- Authentication and authorization mechanisms to ensure secure access.

## Testing

The testing will be carried out continuously during the project to ensure robustness and functionality:

- Unit testing (Java: JUnit / C#: NUnit).
- Integration testing to check interactions between client and server.
- Manual testing of UI components and learning flows for usability and accessibility checks.
- Security testing with emphasis on authentication and authorization.

## Planning and Management

To facilitate organized collaboration and progress monitoring:

- Git with GitHub for version control, feature branching, and code reviews.
- Task distribution and workload management will be done using a Kanban board (Figma/GitHub Projects).
- Regular meetings to check progress, resolve problems, and plan next steps.
- Documentation will be written in a formal academic style, with correct referencing and adherence to plagiarism standards.

---

# Time Schedule

## Final Deadline

Date: December 19, 2025

## Milestones

1. **Weekly Reporting and Task Assignment**  
   When: Every Sunday  
   Details: Submit a weekly report on the project’s progress and assign new tasks for the upcoming week to maintain continuous progress and team accountability.

2. **Weekly Meeting**  
   When: Once per week  
   Platform: Meetings will be conducted either via Discord or at school.  
   Purpose: These meetings will act as checkpoints to discuss progress, address challenges, and adjust tasks as necessary.

3. **Completion of Formal Project Part**  
   Target Date: End of November 2025  
   Details: Aim to complete the formal writing and documentation aspect by this date, allowing time for final revisions before the deadline.

## Expected Time Commitment Based on 10 ECTS

Each student is expected to contribute a total of 275 hours to meet the 10 ECTS workload requirement, with increased hours in October and November to reduce the workload in December.

### Breakdown of Hours for 10 ECTS

- **October:**
  - Weekly commitment: 16 hours per student
  - Total for October: 4 × 16 = 64 hours per student

- **November:**
  - Weekly commitment: 16 hours per student
  - Total for November: 4 × 16 = 64 hours per student

- **December (up to December 20):**
  - Remaining: 147 hours
  - Weekly commitment: 147 / 3 ≈ 49 hours per student
  - Total for December: 3 × 49 = 147 hours per student

### Total Hours Calculation for 10 ECTS

- Total Hours: 275 hours per student
- Calculation: Total hours = 10 × 27.5 = 275 hours

---

# Risk Assessment

| Risk | \thead{Likelihood} | \thead{Severity} | Normalized Product | Preventive Actions | Identifiers | Responsible |
|:------------|:-------:|:-------:|:-------:|:-------------------|:-----------------|:-----------:|
| Limited resources        | 3 | 3 | 2.3 | Prioritize essential features, manage scope carefully, and allocate workload among team members | Missed deadlines, unfinished features, team burnout | Eduard |
| Poor communication       | 3 | 2 | 1.7 | Use clear communication channels, hold regular sync meetings, document decisions | Misunderstandings in tasks, duplicated work | Piotr |
| Unrealistic expectations | 2 | 3 | 1.5 | Define realistic milestones, align scope with resources, clarify goals early | Frequent re-scoping, dissatisfaction from stakeholders | Alexandru |
| Knowledge gaps           | 2 | 2 | 1.5 | Encourage skill-sharing, study relevant topics, consult supervisors when needed | Incorrect implementation, delays in development | Giullermo |
| Unclear requirements     | 3 | 3 | 2.3 | Refine problem statement with stakeholders, document requirements clearly | Confusion during development, frequent rework | Ibrahim |

---

# References

- ReferencesBurke, C. (2023, October 4). Why Is Education Important? The Power Of An Educated Society. Unity Environmental University; Unity Environmental University. https://unity.edu/articles/why-education-is-important/

- Duolingo. (2025). Company Strategy Overview | Duolingo, Inc. Duolingo, Inc. https://investors.duolingo.com/company-strategy-overview-0

- Gender gaps in educational attainment and outcomes remain: Gender Equality in a Changing World. (2025). OECD. https://www.oecd.org/en/publications/gender-equality-in-a-changing-world_e808086f-en/full-report/gender-gaps-in-educational-attainment-and-outcomes-remain_33ea8a2f.html

- Lin, M.-H., Chen, H.-C., & Liu, K.-S. (2017). A Study of the Effects of Digital Learning on Learning Motivation and Learning Outcome. EURASIA Journal of Mathematics, Science and Technology Education, 13(7), 3553–3564. https://doi.org/10.12973/eurasia.2017.00744a

- NAICU - NAICU - Improves Quality of Life. (2025). Naicu.edu. https://www.naicu.edu/research/shaping-lives-and-anchoring-communities/improves-quality-of-life

- OECD. (2024). Early Learning and Child Well-being. OECD. https://www.oecd.org/en/publications/early-learning-and-child-well-being_3990407f-en.html

- Organisation For Economic Co-Operation And Development (OECD). (2020). PISA 2018 Results (Volume IV) : Are Students Smart about Money. Oecd.

- Poggi, A. (2025, April 4). The Digital Divide: A Barrier to Social, Economic and Political Equity | ISPI. ISPI. https://www.ispionline.it/en/publication/the-digital-divide-a-barrier-to-social-economic-and-political-equity-204564

- PTI. (2025, March 2). 40% global population doesn’t have access to education in language they understand: UNESCO. Deccan Herald. https://www.deccanherald.com/world/40-global-population-doesnt-have-access-to-education-in-language-they-understand-unesco-3428194

- Singh, J., Fernando, Z. T., & Chawla, S. (2015). LearnWeb-OER: Improving Accessibility of Open Educational Resources. ArXiv.org. https://arxiv.org/abs/1509.02739

- World Bank Group. (2025, January 29). Digital Pathways for Education: Enabling Greater Impact for All. World Bank; World Bank Group. https://www.worldbank.org/en/topic/edutech/publication/digital-pathways-education-enabling-learning-impact
