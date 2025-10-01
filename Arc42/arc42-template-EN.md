# Table of contents
1. [Chapter 1: Introduction and Goals](#chapter-1-introduction-and-goals)
2. [Chapter 2: Architecture Constraints](#chapter-2-architecture-constraints)
3. [Chapter 3: Context and Scope](#chapter-3-Context-and-Scope)
4. [Chapter 4: Solution Strategy](#chapter-4-Solution-Strategy)
5. [Chapter 5: Building Block View](#chapter-5-building-block-view)
6. [Chapter 6: Runtime View](#chapter-6-runtime-view)
7. [Chapter 7: Deployment View](#chapter-7-deployment-view)
8. [Chapter 8: Cross-cutting Concepts](#chapter-8-cross-cutting-concepts)
9. [Chapter 9: Architecture Decisions](#chapter-9-architecture-decisions)
10. [Chapter 10: Quality Requirements](#chapter-10-quality-requirements)
11. [Chapter 11: Risks and Technical Debts](#chapter-11-risks-and-technical-debts)
12. [Chapter 12: Glossary](#chapter-12-glossary)


# Chapter 1: Introduction and Goals
Unibar Locator er en mobilapplikation, der henvender sig til studerende med det formål at fremme sociale interaktioner. Applikationen præsenterer både kort- og listevisning af nærliggende lokationer og giver yderligere information om prisniveau, antal tilstedeværende studiekammerater, brugervurdering samt rutevejledning. Den er designet til at styrke det sociale liv blandt studerende, øge trivsel og reducere frafald fra uddannelserne.

## Requirements Overview
| ID | User stories | Requirements |
| --- | -------- | ------- |
| US1 | Som studerende ønsker jeg en oversigt over alle lokationer der serverer alkohol, for at have et nemt overblik. | Applikationen skal vise en liste eller et kort med tilgængelige lokationer. |
| US2 | Som studerende vil jeg kunne vælge rute via maps, så jeg hurtigere kan finde vej. | Applikationen skal understøtte integration med korttjenester for at vise vej. |
| US3 | Som studerende ønsker jeg et anslået prisleje på hver location, så jeg kan vælge ud fra min saldo. | Applikationen skal fremvise et prisestimat ved hver location. |
| US4 | Som studerende vil jeg kunne se hvor mine venner er, så vi nemmere kan finde hinanden. | Applikationen skal understøtte visning af venners tilstedeværelse på lokationer. |
| US5 | Som studerende vil jeg kunne give hver location en rating, så jeg nemmere kan sorterer i mulighederne. |  Brugere skal kunne vurdere og bedømme lokationer. |


**Form** <br>
Link til [FURPS+](FK.md)



## Quality Goals
![Categories of Quality
Requirements](images/1_QualityGoals.png)

**Funktionelt krav og kvalitetsmål**
| Funktionelt krav | Relateret kvalitetsmål |
|------------------|-------------------------|
| F1 – Kort med nærliggende lokationer | Brugervenlighed, Ydelse |
| F2 – Liste sorteret efter afstand | Brugervenlighed, Ydelse |
| F3 – Integration med Maps | Brugervenlighed, Tilgængelighed |
| F4 – Fremvise rute til lokation | Brugervenlighed |
| F5 – Fremvise prisleje | Brugervenlighed |
| F6 – Sortere lokationer efter prisleje | Brugervenlighed |
| F7 – Fremvise venners placering i realtid | Pålidelighed, Privatliv |
| F8 – Slå deling af placering til/fra | Privatliv, Sikkerhed |
| F9 – Fremvise rating-system | Brugervenlighed |
| F10 – Gennemsnitlig rating | Brugervenlighed |
| F11 – Sortere lokationer efter rating | Brugervenlighed |
| F12 – Give brugere mulighed for at rate | Brugervenlighed, Pålidelighed |


## Stakeholders 
| Stakeholder                       | Rolle / Beskrivelse                                    | Behov / Interesse i systemet                                                        |
|-----------------------------------|--------------------------------------------------------|-------------------------------------------------------------------------------------|
| Studerende                        | Primære brugere af appen                               | Overblik over sociale aktiviteter, brugervenlighed, sociale funktioner, prisniveau  |
| Lokationsejere                    | Ejere af lokationer der serverer alkohol               | Synlighed, korrekt information, mulighed for at opdatere indhold                    |
| Uddannelsesinstitution            | Videregående uddannelsessteder                         | Data om deltagelse, fremme trivsel, integration med eksisterende systemer           |
| Systemadministrator / IT-team     | Ansvarlig for drift og vedligeholdelse                 | Arkitekturforståelse, dokumentation, sikkerhed og stabilitet                        |
| Udviklingsteam                    | Implementering og videreudvikling                      | Detaljeret arkitektur, kodebase, dokumentation, beslutningsinput                    |
| Designere / UX-ansvarlige         | Ansvarlige for brugeroplevelse                         | Forståelse af funktioner og begrænsninger, input til arkitekturvalg                 |
| Marketing / Kommunikation         | Promovering af appen                                   | Information om funktioner, brugerdemografi, branding                                |
| Eksterne tjenesteudbydere         | Korttjenester, notifikationer, betalingsgateways       | API-adgang, tekniske krav, kompatibilitet                                           |
| Forældre / værger (indirekte)     | Bekymrede for studerendes sikkerhed og trivsel         | Informationssikkerhed og datahåndtering                                             |



# Chapter 2: Architecture Constraints
For at sikre, at udviklingen af Unibar Locator følger både tekniske og organisatoriske krav, er det vigtigt at identificere de begrænsninger, som arkitekten og udviklingsteamet skal tage hensyn til. 
Constraints hjælper med at afklare, hvor der er frihed i designvalg, og hvor specifikke beslutninger er påkrævet. 

## Constraints
| ID | Type     | Beskrivelse                                                                                           |  US | Kommentar                                                   |
|----|----------|-------------------------------------------------------------------------------------------------------|-----|-------------------------------------------------------------|
| C1 | Teknisk  | Applikationen skal understøtte integration med korttjenester for rutevejledning.                      | US2 | Påvirker valg af SDK’er, API’er og dataformat.              |
| C2 | Teknisk  | Applikationen skal kunne vise både liste- og kortvisning af lokationer.                               | US1 | Kræver fleksibel UI-arkitektur og passende datamodel.       |
| C3 | Teknisk  | Applikationen skal vise anslået prisleje for hver lokation.                                           | US3 | Kræver standardisering af prisdata og mulig lokal lagring.  |
| C4 | Teknisk  | Applikationen skal kunne vise venners tilstedeværelse på lokationer.                                  | US4 | Kræver realtidsdatahåndtering og brugergodkendelse.         |
| C5 | Teknisk  | Brugere skal kunne bedømme og give rating til lokationer.                                             | US5 | Kræver database-understøttelse for ratings og feedback.     |
| C6 | Organisatorisk | Udviklingen skal følge virksomhedens programmeringsstandarder                                   | Alle | Sikrer konsistens i kodebase og integration med backend.   |
| C7 | Organisatorisk | Projektet skal følge gældende GDPR-regler ved håndtering af brugerdata.                         | US4, US5 | Påvirker design af autentifikation, anonymisering og datalagring. |


# Chapter 3: Context and Scope

## Business Context
Systemet, Unibar locator, er en mobilapplikation, der forbinder studerende med nærliggende lokationer og medstuderende for at fremme sociale interaktioner. Systemet opretter dermed en forbindelse mellem lokationsejere og studerende, hvor studernede er tilknyttet en uddannelsesinstutioner. Applikationen modtager forespørgsler, processerer data og leverer relevant information.

### Business Context Diagram
![Business Context Diagram](images/3_BuisnessDiagram.png)

### Domain Interfaces Table

| Communication Partner        | Inputs from Partner                                      | Outputs to Partner      |
|------------------------------|---------------------------------------------------------|-------------------------|
| Studerende                   | Søgeforespørgsler, rutevalg, rating, venneplacering     | Liste/kort med lokationer, rutevejledning, venners placering, prisestimat, ratings |
| Lokationsejere               | Opdatering af information (åbningstid, priser, tilbud)  | Bekræftelse af opdatering, statistik over besøg      |
| Uddannelsesinstitution       | Ingen direkte input                                     | Anonymiseret data om deltagelse, sociale mønstre     |
| Eksterne tjenester (Maps API)| Lokationsforespørgsler, ruteforespørgsler               | Kortdata, ruter, afstande, estimeret tid             |
| Notifikationstjenester       | Meddelelser til brugere                                 | Push-notifikationer                                  |


Forklaring:
- Studerende interagerer med appen via UI for at finde lokationer, se venner og vælge ruter.
- Lokationsejere kan opdatere deres information via en webportal eller API.
- Uddannelsesinstitutioner modtager kun anonymiserede rapporter for at bevare privatliv.
- Eksterne tjenester leverer tekniske data såsom kort og rutevejledning.

## Technical Context
Systemet kommunikerer med både interne og eksterne tekniske systemer. Dette inkluderer mobile enheder, servere, databaser og tredjeparts-API’er. Den tekniske kontekst beskriver både kanaler og protokoller.

![Techinal Context Diagram](images/3_TechnicalDiagram.png)



# Chapter 4: Solution Strategy
De arkitektoniske beslutninger udgør fundamentet for Unibar Locators systemarkitektur. Valgene her lægger grundlaget for, hvordan systemet struktureres, hvilke teknologier der anvendes, og hvordan kvalitetsmål som brugervenlighed, performance og datasikkerhed opnås. Beslutningerne er truffet med udgangspunkt i problemstillingen, user stories, constraints og ønskede kvalitetsmål.

## 4.2 Arkitektoniske beslutninger

| Beslutning                                  | Type           | Kvalitetsmål                     | Motivation                                                      |
|---------------------------------------------|----------------|----------------------------------|-----------------------------------------------------------------|
| Brug af mobilapplikation (iOS og Android)   | Teknologi      | Brugervenlighed, Tilgængelighed  | Sikre bred tilgængelighed og udnytte funktioner som GPS og push-notifikationer. |
| Integration med tredjeparts korttjeneste    | Teknologi      | Brugervenlighed, Tilgængelighed  | Tilbyde rutevejledning og kortvisning (F1, F3, F4) uden at skulle udvikle egen løsning. |
| Client-Server arkitektur                    | Arkitektur     | Pålidelighed, Ydelse             | Appen opdeles i frontend og backend for skalerbarhed og realtidsopdateringer (F7). |
| Backend: C# og MySQL                        | Teknologi      | Pålidelighed, Ydelse             | C# anvendes for at følge interne standarder; MySQL til datalagring (F3, F5, F7, F12). |
| UI/UX design med liste- og kortvisning      | Arkitektur     | Brugervenlighed, Ydelse          | Opfylder user stories om nem oversigt over lokationer (US1, US2) og fleksibel navigation (F1, F2, F6, F9, F11). |
| Realtidsfunktionalitet til venners lokation | Kvalitet       | Pålidelighed, Privatliv          | Implementeres med push-notifikationer og periodisk opdatering (F7), samtidig med at GDPR overholdes (F8). |
| Ratings og feedback på lokationer           | Funktionalitet | Brugervenlighed, Pålidelighed    | Brugere kan bedømme lokationer (F9, F10, F11, F12), hvilket understøtter sociale beslutninger og sortering. |
| Udviklingsproces: Scrum                     | Organisatorisk | Brugervenlighed, Pålidelighed    | Iterativ udvikling og hurtig feedback sikrer, at kvalitetsmål opnås løbende. |
| Overholdelse af GDPR                        | Organisatorisk / Teknologi | Privatliv, Sikkerhed | Alle brugerdata håndteres med samtykke og anonymisering, hvilket sikrer brugertrivsel (F7, F8). |

**4.3 Kommentarer**
- Beslutningerne balancerer tekniske muligheder, organisatoriske krav og lovgivningsmæssige begrænsninger.  
- Tabellen viser, hvordan arkitekturen understøtter de vigtigste kvalitetsmål og user stories.  
- Dette fundament bruges i de følgende kapitler til at designe komponentstruktur, datamodel og sikkerhedsløsninger.

# Chapter 5: Building Block View

## Whitebox Overall System
Systemet er opdelt i klart adskilte byggesten for at sikre skalerbarhed, genbrugelighed og nem vedligeholdelse. Denne opdeling giver et abstrakt overblik over systemet uden at afsløre implementeringsdetaljer.

**Overview Diagram**  
![Overview diagram](images/1_Overviewdiagram.png)


## Level 1 - Contained Building Blocks
Building blocks notes
Aktivitets diagram niveau 1
Use case niveau 2
Sekvens  niveau 3
Klasse diagram niveau 4

| Name                   | Responsibility |
|------------------------|----------------|
| Mobilklient            | Viser kort og liste over lokationer, håndterer brugerinteraktion og lokal data caching. Integrerer med korttjeneste og push-notifikationer. |
| Backend API            | Håndterer brugerforespørgsler, venners placering, ratings, prisdata og autentifikation. |
| Database               | Gemmer lokationer, brugerdata, ratings, priser og sessionsdata. |
| Maps Integration       | Leverer kort, ruter og distanceberegning til mobilklienten. |
| Notification Service   | Sender push-notifikationer om venners tilstedeværelse og opdateringer. |

### Important Interfaces

| Interface               | Description |
|-------------------------|------------|
| REST API                | Kommunikerer mellem mobilklient og backend; håndterer CRUD-operationer for lokationer, ratings og brugere. |
| Maps                    | Mobilklienten bruger Maps til kortvisning og rutevejledning. |
| Push Notification API   | Backend sender realtidsopdateringer til brugernes mobilklienter. |

## Level 2 – Internal Structure of Key Building Blocks

### White Box *Mobilklient*

- **Purpose & Responsibility:** Præsenterer lokationer via kort- og listevisning, håndterer brugerinput, viser venners placering og ratings.  
- **Interfaces:** REST API til backend, Maps, Push Notification API.  
- **Quality & Performance:** Skal være responsiv, understøtte offline caching af lokationer.  
- **Directory & File Location:** /app/mobile  
- **Fulfilled Requirements:** US1, US2, US3, US4, US5; kvalitetsmål F1-F12.

### White Box *Backend API*

- **Purpose/Responsibility:** Behandler forespørgsler fra klienter, autentificerer brugere, styrer sessionsdata og realtidsdata.  
- **Interfaces:** REST API, Database connection, Notification Service.  
- **Quality/Performance:** Høj tilgængelighed, lav responstid.  
- **Directory/File Location:** /app/backend  
- **Fulfilled Requirements:** US1-US5; kvalitetsmål F3, F4, F7, F12.

### White Box *Database*

- **Purpose/Responsibility:** Lagrer lokationer, brugere, ratings, prisniveauer og sessionsinformation.  
- **Interfaces:** SQL queries fra Backend API.  
- **Quality/Performance:** ACID-kompatibel, sikker lagring, backup-muligheder.  
- **Directory/File Location:** /db  
- **Fulfilled Requirements:** US1-US5; kvalitetsmål F5-F12.

### White Box *Maps Integration*

- **Purpose/Responsibility:** Tilbyder kortdata, rutevejledning og afstandsberegning.  
- **Interfaces:** Maps til mobilklient.  
- **Fulfilled Requirements:** US2; kvalitetsmål F3, F4.

### White Box *Notification Service*

- **Purpose/Responsibility:** Sender push-notifikationer om venners tilstedeværelse og app-opdateringer.  
- **Interfaces:** Push Notification API til mobilklient.  
- **Quality/Performance:** Realtidsopdateringer med høj pålidelighed.  
- **Fulfilled Requirements:** US4; kvalitetsmål F7, F8.

## Level 3 – Internal Structure of Mobilklient 






# Chapter 6: Runtime View

::: formalpara-title
**Contents**
:::

The runtime view describes concrete behavior and interactions of the
system's building blocks in form of scenarios from the following areas:

-   important use cases or features: how do building blocks execute
    them?

-   interactions at critical external interfaces: how do building blocks
    cooperate with users and neighboring systems?

-   operation and administration: launch, start-up, stop

-   error and exception scenarios

Remark: The main criterion for the choice of possible scenarios
(sequences, workflows) is their **architectural relevance**. It is
**not** important to describe a large number of scenarios. You should
rather document a representative selection.

::: formalpara-title
**Motivation**
:::

You should understand how (instances of) building blocks of your system
perform their job and communicate at runtime. You will mainly capture
scenarios in your documentation to communicate your architecture to
stakeholders that are less willing or able to read and understand the
static models (building block view, deployment view).

::: formalpara-title
**Form**
:::

There are many notations for describing scenarios, e.g.

-   numbered list of steps (in natural language)

-   activity diagrams or flow charts

-   sequence diagrams

-   BPMN or EPCs (event process chains)

-   state machines

-   ...

See [Runtime View](https://docs.arc42.org/section-6/) in the arc42
documentation.

## \<Runtime Scenario 1> {#__runtime_scenario_1}

-   *\<insert runtime diagram or textual description of the scenario>*

-   *\<insert description of the notable aspects of the interactions
    between the building block instances depicted in this diagram.\>*

## \<Runtime Scenario 2> {#__runtime_scenario_2}

## ... {#_}

## \<Runtime Scenario n> {#__runtime_scenario_n}




# Chapter 7: Deployment View 

::: formalpara-title
**Content**
:::

The deployment view describes:

1.  technical infrastructure used to execute your system, with
    infrastructure elements like geographical locations, environments,
    computers, processors, channels and net topologies as well as other
    infrastructure elements and

2.  mapping of (software) building blocks to that infrastructure
    elements.

Often systems are executed in different environments, e.g. development
environment, test environment, production environment. In such cases you
should document all relevant environments.

Especially document a deployment view if your software is executed as
distributed system with more than one computer, processor, server or
container or when you design and construct your own hardware processors
and chips.

From a software perspective it is sufficient to capture only those
elements of an infrastructure that are needed to show a deployment of
your building blocks. Hardware architects can go beyond that and
describe an infrastructure to any level of detail they need to capture.

::: formalpara-title
**Motivation**
:::

Software does not run without hardware. This underlying infrastructure
can and will influence a system and/or some cross-cutting concepts.
Therefore, there is a need to know the infrastructure.

Maybe a highest level deployment diagram is already contained in section
3.2. as technical context with your own infrastructure as ONE black box.
In this section one can zoom into this black box using additional
deployment diagrams:

-   UML offers deployment diagrams to express that view. Use it,
    probably with nested diagrams, when your infrastructure is more
    complex.

-   When your (hardware) stakeholders prefer other kinds of diagrams
    rather than a deployment diagram, let them use any kind that is able
    to show nodes and channels of the infrastructure.

See [Deployment View](https://docs.arc42.org/section-7/) in the arc42
documentation.

## Infrastructure Level 1 {#_infrastructure_level_1}

Describe (usually in a combination of diagrams, tables, and text):

-   distribution of a system to multiple locations, environments,
    computers, processors, .., as well as physical connections between
    them

-   important justifications or motivations for this deployment
    structure

-   quality and/or performance features of this infrastructure

-   mapping of software artifacts to elements of this infrastructure

For multiple environments or alternative deployments please copy and
adapt this section of arc42 for all relevant environments.

***\<Overview Diagram>***

Motivation

:   *\<explanation in text form>*

Quality and/or Performance Features

:   *\<explanation in text form>*

Mapping of Building Blocks to Infrastructure

:   *\<description of the mapping>*

## Infrastructure Level 2 {#_infrastructure_level_2}

Here you can include the internal structure of (some) infrastructure
elements from level 1.

Please copy the structure from level 1 for each selected element.

### *\<Infrastructure Element 1>* {#__emphasis_infrastructure_element_1_emphasis}

*\<diagram + explanation>*

### *\<Infrastructure Element 2>* {#__emphasis_infrastructure_element_2_emphasis}

*\<diagram + explanation>*

...

### *\<Infrastructure Element n>* {#__emphasis_infrastructure_element_n_emphasis}

*\<diagram + explanation>*

# Chapter 8: Cross-cutting Concepts

::: formalpara-title
**Content**
:::

This section describes overall, principal regulations and solution ideas
that are relevant in multiple parts (= cross-cutting) of your system.
Such concepts are often related to multiple building blocks. They can
include many different topics, such as

-   models, especially domain models

-   architecture or design patterns

-   rules for using specific technology

-   principal, often technical decisions of an overarching (=
    cross-cutting) nature

-   implementation rules

::: formalpara-title
**Motivation**
:::

Concepts form the basis for *conceptual integrity* (consistency,
homogeneity) of the architecture. Thus, they are an important
contribution to achieve inner qualities of your system.

Some of these concepts cannot be assigned to individual building blocks,
e.g. security or safety.

::: formalpara-title
**Form**
:::

The form can be varied:

-   concept papers with any kind of structure

-   cross-cutting model excerpts or scenarios using notations of the
    architecture views

-   sample implementations, especially for technical concepts

-   reference to typical usage of standard frameworks (e.g. using
    Hibernate for object/relational mapping)

::: formalpara-title
**Structure**
:::

A potential (but not mandatory) structure for this section could be:

-   Domain concepts

-   User Experience concepts (UX)

-   Safety and security concepts

-   Architecture and design patterns

-   \"Under-the-hood\"

-   development concepts

-   operational concepts

Note: it might be difficult to assign individual concepts to one
specific topic on this list.

![Possible topics for crosscutting
concepts](images/08-concepts-EN.drawio.png)

See [Concepts](https://docs.arc42.org/section-8/) in the arc42
documentation.

## *\<Concept 1>* {#__emphasis_concept_1_emphasis}

*\<explanation>*

## *\<Concept 2>* {#__emphasis_concept_2_emphasis}

*\<explanation>*

...

## *\<Concept n>* {#__emphasis_concept_n_emphasis}

*\<explanation>*

# Chapter 9: Architecture Decisions

::: formalpara-title
**Contents**
:::

Important, expensive, large scale or risky architecture decisions
including rationales. With \"decisions\" we mean selecting one
alternative based on given criteria.

Please use your judgement to decide whether an architectural decision
should be documented here in this central section or whether you better
document it locally (e.g. within the white box template of one building
block).

Avoid redundancy. Refer to section 4, where you already captured the
most important decisions of your architecture.

::: formalpara-title
**Motivation**
:::

Stakeholders of your system should be able to comprehend and retrace
your decisions.

::: formalpara-title
**Form**
:::

Various options:

-   ADR ([Documenting Architecture
    Decisions](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions))
    for every important decision

-   List or table, ordered by importance and consequences or:

-   more detailed in form of separate sections per decision

See [Architecture Decisions](https://docs.arc42.org/section-9/) in the
arc42 documentation. There you will find links and examples about ADR.

# Chapter 10: Quality Requirements
Dette kapitel beskriver alle relevante kvalitetskrav for applikationen. Se 
De mest kritiske kvalitetskrav er allerede beskrevet i sektion 1.2 (Kvalitetsmål) og refereres her.  
Derudover indeholder kapitlet kvalitetskrav med lavere prioritet, som er “nice-to-have” og ikke skaber høj risiko, hvis de ikke opfyldes fuldt ud.

## Quality Requirements Overview
| Kategori                 | Underkategori                           | Prioritet | Relaterede scenarier / Noter |
|--------------------------|-----------------------------------------|-----------|------------------------------|
| **Brugervenlighed**      | Intuitiv navigation                     | Høj       | QS1, QS2                     |
|                          | Hurtig kontooprettelse                  | Mellem    | QS3                          |
|                          | Klar informationsvisning                | Mellem    | QS4                          |
| **Tilgængelighed**       | Fungerer på forskellige enheder         | Høj       | QS15                         |
|                          | Offline visning af visse data           | Lav       | QS17 (nice-to-have)          |
| **Pålidelighed**         | Stabil opdatering af venners placering  | Høj       | QS7, QS8                     |
|                          | Håndtering af netværksfejl              | Mellem    | QS9                          |
| **Ydelse**               | Hurtige indlæsningstider                | Høj       | QS5, QS6                     |
|                          | Lav latenstid for realtidsdata          | Mellem    | QS7                          |
| **Privatliv**            | Beskyttelse af brugerdata               | Høj       | QS10, QS11                   |
|                          | Valgfri deling af lokation              | Høj       | QS12                         |
| **Sikkerhed**            | Kryptering af følsomme data             | Høj       | QS10                         |
|                          | Adgangskontrol og autentificering       | Lav       | QS18 (nice-to-have)          |

> Bemærk: Underkategorier som QS17 og QS18 er eksempler på mindre prioriterede krav (“nice-to-have”) og skaber ikke høj risiko, hvis de ikke implementeres fuldt ud.


## Quality Scenarios 
| Scenarie ID | Type       | Stimulus / Begivenhed                   | Forventet systemadfærd / Respons                     | Relateret krav / User Story | Prioritet |
|------------|------------|----------------------------------------|--------------------------------------------------------|-----------------------------|-----------|
| QS1        | Brug       | Bruger åbner appen første gang          | Onboarding gennemføres inden for 10 sekunder          | US1, F1                     | Høj       |
| QS2        | Brug       | Bruger navigerer gennem lokationsliste  | UI indlæser ny side inden for 2 sekunder              | US1, F2                     | Høj       |
| QS3        | Brug       | Ny bruger tilmelder sig                 | Registrering gennemføres uden fejl                    | US1, F1                     | Høj       |
| QS4        | Brug       | Bruger ser pris- og ratinginfo          | Data vises korrekt og tydeligt                        | US3, US5                    | Mellem    |
| QS5        | Brug       | App indlæser kort/liste over lokationer | Liste/kort fuldt indlæst inden for 2 sekunder         | US1, F1                     | Høj       |
| QS6        | Brug       | Flere brugere forespørger ruter         | System svarer uden mærkbar forsinkelse                | US2, F3                     | Mellem    |
| QS7        | Brug       | Bruger anmoder om venners realtidsplacering | Opdateret placering vises inden for 5 sekunder    | US4, F7                     | Mellem    |
| QS8        | Ændring    | Backend service opdateres / går ned     | System gendanner automatisk; datakonsistens opretholdes | F7, F8                    | Høj       |
| QS9        | Ændring    | Netværksforbindelse mistes under brug   | System håndterer fejlen og prøver igen automatisk     | F7, F8                      | Lav       |
| QS10       | Brug       | Bruger indsender personlige data        | Data gemmes sikkert med kryptering                    | US4, F8                     | Høj       |
| QS11       | Brug       | Admin tilgår brugeranalyser             | Kun anonymiserede data tilgængelige                   | F7, F8                      | Høj       |
| QS12       | Brug       | Bruger slår lokationsdeling til/fra     | Lokationsdeling aktiveres/deaktiveres straks          | US4, F8                     | Høj       |
| QS15       | Brug       | App åbnes på forskellige enheder        | Layout og funktionalitet tilpasses korrekt            | F1, F2                      | Mellem    |
| QS17       | Brug       | Bruger forsøger offline adgang          | Begrænset data vises uden netværk                     | Nice-to-have                | Lav       |
| QS18       | Ændring    | Admin opretter ny adgangsrolle          | Adgangskontrol implementeres korrekt                  | Nice-to-have                | Lav       |


# Chapter 11: Risks and Technical Debts 

::: formalpara-title
**Contents**
:::

A list of identified technical risks or technical debts, ordered by
priority

::: formalpara-title
**Motivation**
:::

"Risk management is project management for grown-ups" (Tim Lister,
Atlantic Systems Guild.)

This should be your motto for systematic detection and evaluation of
risks and technical debts in the architecture, which will be needed by
management stakeholders (e.g. project managers, product owners) as part
of the overall risk analysis and measurement planning.

::: formalpara-title
**Form**
:::

List of risks and/or technical debts, probably including suggested
measures to minimize, mitigate or avoid risks or reduce technical debts.

See [Risks and Technical Debt](https://docs.arc42.org/section-11/) in
the arc42 documentation.

# Chapter 12: Glossary

::: formalpara-title
**Contents**
:::

The most important domain and technical terms that your stakeholders use
when discussing the system.

You can also see the glossary as source for translations if you work in
multi-language teams.

::: formalpara-title
**Motivation**
:::

You should clearly define your terms, so that all stakeholders

-   have an identical understanding of these terms

-   do not use synonyms and homonyms

A table with columns \<Term> and \<Definition>.

Potentially more columns in case you need translations.

See [Glossary](https://docs.arc42.org/section-12/) in the arc42
documentation.

+-----------------------+-----------------------------------------------+
| Term                  | Definition                                    |
+=======================+===============================================+
| *\<Term-1>*           | *\<definition-1>*                             |
+-----------------------+-----------------------------------------------+
| *\<Term-2>*           | *\<definition-2>*                             |
+-----------------------+-----------------------------------------------+
