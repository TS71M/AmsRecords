using AmsRecords.Assistant;

namespace AmsRecords.Assistant;

public static class AssistantProductKnowledge
{
    static readonly IReadOnlyList<SupportAssistantKnowledgeSnippetDto> Core =
    [
        Snippet("Workflow: GP-scaled fertilizer programmes", """
            The WebApp Fertilizer Planner has GP-scaled applications for recurring, user-selected nitrogen programmes.
            The dose belongs to each application. A four-week reference of 2 g N/m² means 0.5 g per weekly application
            or 1 g per fortnightly application at 100% GP. Five weekly dates keep five doses; three fortnightly dates
            keep three doses. Never shrink the dose to fit a calendar-month total. Other intervals require an explicit
            per-application dose. Dates follow the chosen interval continuously from an explicit first date through
            the reviewed last date, including leap days and month boundaries.
            For the 40% cutoff programme, each dose is baseline N times captured monthly GP/100 when GP is at least 40%;
            below 40% it supplies zero nitrogen. Missing GP is unknown. Zero-dose dates preserve cadence and receive
            no application record. Excluded doses are not redistributed. This is a user's planning rule, not a universal
            fertilizer recommendation or a live forecast. An explicitly strict monthly cap uses monthly allocation instead.
            Choose the exact frozen product with verified N analysis; never assume ammonium sulphate's percentage.
            Preview dates, captured GP, requested/delivered nitrogen and product quantities before saving.
            Save adds the product to the zone's used products when needed, with dated plan items in one transaction.
            Products absent from the frozen inputs must first be captured and reviewed through Plans & history.
            Planning edit permission and field scope are required; recording rights do not grant them.
            Existing identifiable GP programmes in editable drafts can change between weekly and fortnightly while
            preserving their saved four-week N reference and cutoff. The server derives the new per-application dose.
            Select the exact product, zone and explicit first/last dates; preview existing and replacement applications,
            then confirm Save. Changed rates, mixed programmes, preparation details, executions and operational history
            block bulk replacement. Never claim a capability question has changed a programme.
            """),
        Snippet("Workflow: alternative fertilizer programmes", """
            The WebApp alternative fertilizer plan editor creates a separate same-year draft from a selected revision.
            When the invariant is unclear, ASK whether to preserve annual nitrogen or the four-week nitrogen reference.
            These are different constraints; never silently pick one. Annual mode balances against the intended annual
            nitrogen of explicitly reviewed source products in one zone. Other products and zones are copied unchanged.
            Four-week mode requires an identifiable saved GP reference and can change annual totals with new dates.
            Alternatives support changed intervals and verified replacement products or explicit nitrogen-share blends.
            Shares are percentages of N, not product mass. Missing or ambiguous products and shares need review;
            never infer a nominal formulation, stock or blend. Captured GP weights dates and below-cutoff dates get zero N.
            Preview source/proposed nitrogen, delivered rounding difference, other captured nutrients, dates, analyses
            and quantities. Annual mode blocks a difference beyond 0.001 g N/m². Matching N alone does not establish
            product suitability, release pattern, other nutrients or tank compatibility. Confirm Save creates a draft;
            source plans, actual work, operational selection and procurement remain separate. Source/date/basis ambiguity
            requires clarification. Physical area, boundary dates and shared preparation need explicit individual review.
            """),
        Snippet("Workflow: stock-first fertilizer cost alternatives", """
            Explicit cheaper-alternative requests can use a stock-first cost search while preserving annual N, captured
            GP scaling, cutoff and the reviewed interval. Compare currently active, tenant/field-scoped catalogue products
            already captured in the revision. Use usable captured stock first after unchanged planned demand, then compare
            one purchase product at a time for remaining N. This is not a global optimum across every possible purchasing blend.
            Rank estimated new-pack purchasing cost first, with material value as a tie breaker. Show both values, stock used,
            shortfall, rounded new packs, savings and other nutrients against the source. Stock is not free material.
            Zero/unknown prices, ambiguous units, missing packaging and unknown stock do not establish savings. Products with
            stock but incomplete inputs require review before searching. If no cheaper proposal is found, say so and disable
            creation in cheaper mode. Explicit review and confirmation create a separate draft; no stock reservations, orders,
            actuals or operational selection changes occur. Refresh frozen price/stock evidence explicitly when needed.
            Native clients can explain this rule; the GP programme editor is a WebApp workflow.
            """),
        Snippet(
            "Field risk badge interpretation",
            """
            The field/course overview risk badge shows the highest available risk across today and the next five forecast days, not just today's risk, and is not a sum or average.
            Its compact visible text is only the risk level and trend arrow. On the WebApp, hover over the badge for the causing risks; on mobile, tap it to show those risks and optionally open the full forecast.
            Multiple causes can share the highest severity. Do not infer which disease or condition caused a particular badge from its colour alone; use the supplied current risk data or say the causes are unavailable.
            The forecast-period explanation belongs in page information, not repeated badge tooltips.
            Daily forecast arrows are a separate comparison with the next day: up above +1 percentage point, down below -1, steady within the inclusive ±1 band. The last day or missing comparison data has no trend available, shown as a dash.
            A predicted high risk is a planning/scouting signal, not confirmation of disease.
            """),
        Snippet(
            "Agronomy Manager glossary",
            """
            Agronomy Manager is built for golf course and turf management.
            An IBU is the organisation/workspace.
            A field is the managed site or course record.
            Areas are turf or management areas within a field.
            Holes belong to a field and can have their own hole record, image, and surface assignments.
            Surfaces are assigned per hole and usually depend on areas and holes existing first.
            Use course, field, turf, playing surface, area, hole, green, tee, fairway, rough, soil test, water test, weather, GDD, GP, GTS, risk, diary, and Procural language.
            Avoid generic crop/farm wording unless the user or page explicitly uses it.
            """),
        Snippet(
            "UI and permissions",
            """
            Many pages use icon-only actions. Describe the icon location and purpose when the visible label is missing.
            If a create/edit/delete button is missing, the most likely causes are role permissions, current field access, or the record needing to be saved first.
            Supervisors and admins usually see setup and edit actions that standard users may not see.
            When answering how-to questions, give task steps first, then mention likely permission or prerequisite issues.
            """),
        Snippet(
            "Information buttons",
            """
            The circular or icon information button on a page contains page-specific guidance.
            The support assistant receives that information as knowledge and should use it as the preferred source for current-page questions.
            If the user asks what a page, value, table, or button means, answer from the information-button content when available.
            """)
    ];

    static readonly IReadOnlyList<SupportAssistantKnowledgeSnippetDto> FieldWorkflows =
    [
        Snippet(
            "Workflow: create a field",
            """
            To create a new field, open the Fields page and use the add/new-item button in the toolbar.
            This opens FieldForm/Add Field.
            Choose Field Type first because it controls which sections are relevant.
            Fill the core field details such as name, jurisdiction, climate zone, construction/foundation year, units, and optional image.
            Save the field before adding related address and location, contact details, areas, holes, surfaces, tests, weather setup, or Procural settings.
            If the add field button is not visible, the user probably needs supervisor/admin permission.
            """),
        Snippet(
            "Workflow: field hub",
            """
            The Field Hub is the main navigation point for a selected field.
            Use it to open surfaces, soil tests, water tests, weather, diary, and field Procural.
            Supervisors can usually jump from the hub to field settings, address and location, or contact details.
            """)
    ];

    static readonly IReadOnlyList<SupportAssistantKnowledgeSnippetDto> SurfaceWorkflows =
    [
        Snippet(
            "Workflow: add or replace a hole image",
            """
            To add or replace an image for a hole, edit the hole record itself.
            From the Surfaces area, open the Holes page/list, then edit the target hole.
            On the Hole form, use the image upload/replace area, choose or drop the image file, then save.
            The edit action on a hole card in the Surfaces page may edit surface assignments, not necessarily the hole photo.
            """),
        Snippet(
            "Workflow: assign surfaces to holes",
            """
            Areas and holes should exist before assigning surfaces.
            On the Surfaces page, open the edit action for the relevant hole card to assign or change the surfaces for that hole.
            Surface totals summarize the current assignments for the selected field.
            """)
    ];

    static readonly IReadOnlyList<SupportAssistantKnowledgeSnippetDto> WeatherWorkflows =
    [
        Snippet(
            "Weather concepts",
            """
            Weather pages use stored weather observations, forecasts, summaries, and field-specific visualization units.
            GDD, GP, GTS, risks, and stress views are planning aids and should be checked against field reality.
            If weather-derived values are missing, likely causes include missing weather summaries, incomplete backfill, no selected field, or the selected year/date not having enough data.
            """),
        Snippet(
            "GTS interpretation",
            """
            GTS means Gruenlandtemperatursumme, a weighted spring temperature sum used to judge seasonal development and timing windows.
            The series starts on January 1.
            January contributes with weight 0.5, February with 0.75, and March onward with weight 1.0.
            Current GTS is the running weighted accumulation for the selected year and as-of date.
            Stage and threshold badges come from configured GTS rules and timing windows.
            Weed timing windows are management guidance only and should be checked against field conditions.
            """)
    ];

    static readonly IReadOnlyList<SupportAssistantKnowledgeSnippetDto> TestWorkflows =
    [
        Snippet(
            "Workflow: soil tests",
            """
            Soil tests are managed per field and surface.
            A user can create or edit a soil test manually, add one row per analyte, or use import/scan helpers to prefill values from a lab report.
            The user should review detected field name, date, matched surfaces, analytes, units, warnings, and OCR preview before saving imported tests.
            """),
        Snippet(
            "Workflow: water tests",
            """
            Water tests store one water analysis report.
            Fill or confirm the report header first, then enter or import the result rows.
            A report can be excluded from comparisons when it should remain stored but not influence later analysis.
            """)
    ];

    static readonly IReadOnlyList<SupportAssistantKnowledgeSnippetDto> ProcuralWorkflows =
    [
        Snippet(
            "Procural workflow",
            """
            Procural is Agronomy Manager's procurement workspace.
            Field Procural is field-specific; the IBU Procural workspace is shared across the organisation.
            Procurement routing may be central, field-level, or hybrid depending on field settings and role permissions.
            Products, suppliers, labels, nutrients, supplements, requisitions, and settings are connected pieces of the Procural workflow.
            """)
    ];

    static readonly IReadOnlyList<SupportAssistantKnowledgeSnippetDto> DiagnosticWorkflows =
    [
        Snippet(
            "Diagnostics workflow",
            """
            Diagnostics can include AI-generated findings that need supervisor review.
            The review queue is where supervisors confirm, correct, or reject new AI diagnoses before they become trusted operational records.
            A high risk or AI suggestion is not proof of disease; it should guide scouting and review.
            """)
    ];

    // Product documentation is not evidence of records, enabled modules or user permissions.
    public static IReadOnlyList<SupportAssistantKnowledgeSnippetDto> All { get; } = Core
        .Concat(FieldWorkflows).Concat(SurfaceWorkflows).Concat(WeatherWorkflows)
        .Concat(TestWorkflows).Concat(ProcuralWorkflows).Concat(DiagnosticWorkflows).ToArray();

    public static IReadOnlyList<SupportAssistantKnowledgeSnippetDto> ForPage(string? area, string? page)
    {
        var snippets = new List<SupportAssistantKnowledgeSnippetDto>(Core);
        var route = $"{area}:{page}";

        if (Contains(route, "Field:/FieldsIndex", "Field:/FieldForm", "Field:/FieldHub"))
            snippets.AddRange(FieldWorkflows);

        if (Contains(route, "/Surfaces/", "/Holes/", "Surface"))
            snippets.AddRange(SurfaceWorkflows);

        if (Contains(route, "/Weather/", "Gts", "Gdd", "GPIndex", "Risks"))
            snippets.AddRange(WeatherWorkflows);

        if (Contains(route, "/SoilTests/", "/WaterTests/"))
            snippets.AddRange(TestWorkflows);

        if (Contains(route, "Procural", "Supplier", "Product", "PurchaseRequisition"))
            snippets.AddRange(ProcuralWorkflows);

        if (Contains(route, "Diagnostic", "ReviewQueue"))
            snippets.AddRange(DiagnosticWorkflows);

        return snippets
            .DistinctBy(x => x.Title, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    static SupportAssistantKnowledgeSnippetDto Snippet(string title, string content)
        => new(title, content.Trim(), "SupportKnowledgeCatalog");

    static bool Contains(string value, params string[] needles)
        => needles.Any(needle => value.Contains(needle, StringComparison.OrdinalIgnoreCase));
}
