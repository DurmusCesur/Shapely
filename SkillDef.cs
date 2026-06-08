using System.IO;

namespace CoderAIChat.Core.Skills;

public sealed class SkillDef
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public List<string> Triggers { get; init; } = new();
    public List<string>? Tools { get; init; }
    public string Prompt { get; init; } = string.Empty;
    public string? FilePath { get; init; }
    public string? WhenToUse { get; init; }
    public string? ArgumentHint { get; init; }
    public List<string> Arguments { get; init; } = new();
    public string? Model { get; init; }
    public bool UserInvocable { get; init; } = true;
    public string Context { get; init; } = "inline";
    public string Source { get; init; } = "user";
}

public static class SkillLoader
{
    public static List<SkillDef> LoadAll()
    {
        var skills = new Dictionary<string, SkillDef>();

        foreach (var builtin in BuiltInSkills.All)
            skills[builtin.Name] = builtin;

        var userDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CoderAIChat", "skills");
        if (Directory.Exists(userDir))
            LoadFromDirectory(skills, userDir, "user");

        var projDir = Path.Combine(Directory.GetCurrentDirectory(), ".coderaichat", "skills");
        if (Directory.Exists(projDir))
            LoadFromDirectory(skills, projDir, "project");

        return skills.Values.ToList();
    }

    public static SkillDef? FindByTrigger(string query)
    {
        query = query.Trim();
        if (string.IsNullOrEmpty(query)) return null;
        var firstWord = query.Split()[0];

        foreach (var skill in LoadAll())
        {
            foreach (var trigger in skill.Triggers)
            {
                if (firstWord == trigger || trigger.StartsWith(firstWord + " "))
                    return skill;
            }
        }
        return null;
    }

    public static string SubstituteArguments(string prompt, string args, List<string> argNames)
    {
        var result = prompt.Replace("$ARGUMENTS", args);
        var argValues = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < argNames.Count; i++)
        {
            var placeholder = $"${argNames[i].ToUpperInvariant()}";
            var value = i < argValues.Length ? argValues[i] : "";
            result = result.Replace(placeholder, value);
        }
        return result;
    }

    private static void LoadFromDirectory(Dictionary<string, SkillDef> skills, string dir, string source)
    {
        foreach (var file in Directory.GetFiles(dir, "*.md"))
        {
            try
            {
                var text = File.ReadAllText(file);
                var skill = ParseSkillFile(text, Path.GetFileNameWithoutExtension(file), source, file);
                if (skill is not null)
                    skills[skill.Name] = skill;
            }
            catch { }
        }
    }

    private static SkillDef? ParseSkillFile(string text, string defaultName, string source, string filePath)
    {
        if (!text.StartsWith("---")) return null;
        var end = text.IndexOf("---", 3);
        if (end < 0) return null;

        var fmText = text[3..end];
        var prompt = text[(end + 3)..].Trim();
        var name = defaultName;
        string? description = null, whenToUse = null, argumentHint = null, model = null, context = "inline";
        var triggers = new List<string>();
        var arguments = new List<string>();
        List<string>? tools = null;
        var userInvocable = true;

        foreach (var line in fmText.Split('\n'))
        {
            var colonIdx = line.IndexOf(':');
            if (colonIdx < 0) continue;
            var key = line[..colonIdx].Trim().ToLowerInvariant();
            var val = line[(colonIdx + 1)..].Trim();

            switch (key)
            {
                case "name": name = val; break;
                case "description": description = val; break;
                case "when_to_use": case "when-to-use": whenToUse = val; break;
                case "argument-hint": case "argument_hint": argumentHint = val; break;
                case "model": model = val; break;
                case "context": context = val.ToLowerInvariant() == "fork" ? "fork" : "inline"; break;
                case "user-invocable": case "user_invocable": userInvocable = val.ToLowerInvariant() is not ("false" or "0" or "no"); break;
                case "triggers": triggers = ParseListField(val); break;
                case "tools": case "allowed-tools": tools = ParseListField(val); break;
                case "arguments": arguments = ParseListField(val); break;
            }
        }

        if (triggers.Count == 0) triggers.Add($"/{name}");

        return new SkillDef
        {
            Name = name,
            Description = description ?? "",
            Triggers = triggers,
            Tools = tools,
            Prompt = prompt,
            FilePath = filePath,
            WhenToUse = whenToUse,
            ArgumentHint = argumentHint,
            Arguments = arguments,
            Model = model,
            UserInvocable = userInvocable,
            Context = context,
            Source = source
        };
    }

    private static List<string> ParseListField(string value)
    {
        value = value.Trim();
        if (value.StartsWith('[') && value.EndsWith(']'))
            value = value[1..^1];
        return value.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim().Trim('\'', '"'))
            .Where(s => !string.IsNullOrEmpty(s))
            .ToList();
    }
}

public static class BuiltInSkills
{
    public static readonly List<SkillDef> All = new()
    {
        new SkillDef
        {
            Name = "search",
            Description = "Mandatory playbook for performing targeted web searches to fill knowledge gaps, verify API usage, and ensure best practices in code generation and debugging.",
            Triggers = new List<string> { "/search" },
            Tools = new List<string> 
            {
            "WebSearch", "WebFetch", "Read", "Grep", "Glob" 
            },
            Prompt = """
                This skill is designed to perform targeted web searches to fill knowledge gaps, verify API usage, and ensure best practices in code generation and debugging.
                Follow these best practices when code reviewing thoroughly and provide structured feedback based. Always adhere to these guidelines to ensure robust, efficient, and maintainable scripts:

                ## Domain Restrictions
                - You MUST strictly prioritize and scope your web queries toward:
                * site:forum.dynamobim.com (Dynamo specific logic and nodes)
                * site:thebuildingcoder.typepad.com (Jeremy Tammik's gold standard Revit API guide)
                * site:forums.autodesk.com (Official Autodesk developer community)
                * GitHub repositories containing official Autodesk/Dynamo samples.
                * site:forum.voltadynabim.blogspot.com (Python guru for Dynamo Python scripts)

                - You MUST construct your search queries using strict search operators when possible (e.g., 'site:forum.dynamobim.com transactional error').
                - ALWAYS include the specific target Revit/Dynamo version in the query to isolate deprecated API signatures.

                ## Version Awareness
                - Always append the explicit target Revit/Dynamo version (e.g., 'Revit 2024 Units API', 'Revit 2027') to your searches to capture breaking API changes.
                    
                User context: $ARGUMENTS
                """,
            FilePath = "<builtin>",
            WhenToUse = "Use when encountering an unknown API error, a deprecated method, or when specific code patterns for Revit/Dynamo are requested but undocumented locally.",
            Source = "builtin"
        }, 
        new SkillDef
        {
            Name = "review",
            Description = "Mandatory playbook for performing thorough code reviews to analyze modifications, evaluate risk, and provide structured feedback.",
            Triggers = new List<string> { "/review" },
            Tools = new List<string> 
            { 
            "WebSearch", "WebFetch", "Read", "Grep", "Glob" 
            },
            Prompt = """
                Follow these best practices when code reviewing thoroughly and provide structured feedback based. Always adhere to these guidelines to ensure robust, efficient, and maintainable scripts:

                ## 1. Context & Diff Gathering
                - Identify target files, language environments (Python, C#), and domain context.

                ## 2. Strict Severity Categorization
                You MUST categorize every finding into one of these three strict buckets. This directly controls the system's execution loop:

                - **CRITICAL**: High-severity flaws that WILL halt the process and force a correction cycle.
                * Logic errors, math/geometry calculation flaws, or data corruption risks.
                * Tensor operations shape mismatches or flaws in long-running deep learning pipelines.
                * Security vulnerabilities (exposed secrets, injection risks).
                * Revit API transaction mishandling (unclosed transactions, missing modification scopes).
                * Infinite loops or severe memory/geometry resource leaks.
                
                - **WARNING**: Medium-severity issues that need attention but will NOT block approval.
                * Suboptimal performance (e.g., heavy API loops that could be batched, redundant geometric conversions).
                * Missing edge-case handling or lack of defensive programming.
                * Missing unit tests for newly introduced critical business logic.
                * Outdated or misleading code comments.

                - **SUGGESTION**: Low-severity recommendations for code health.
                * Code style improvements (PEP 8 for Python, styling conventions for C#).
                * Refactoring suggestions for better readability or DRY principles.
                * Alternative, more idiomatic approaches.

                ## 3. Domain-Specific Checklists
                - **If Generic/Data Science Python**: Validate tensor dimensions, matrix alignment, vectorization efficiency, and exception handling boundaries.
                - **If Dynamo/Revit Context**: Verify proper `UnwrapElement` usage, explicit transaction management via `DocumentManager`, and proper disposing of intensive `ProtoGeometry` objects in memory.

                ## 4. Output Structure
                You must deliver your synthesis in the following clear structure:
                
                ### Summary
                [Brief overview of the changes and general quality]

                ### Review Findings
                Use the exact categorization tags: `[CRITICAL]`, `[WARNING]`, or `[SUGGESTION]`.
                - **[CRITICAL]** <File Path>:<Line> - <Description of the issue and why it breaks the logic>
                - **[WARNING]** <File Path>:<Line> - <Description of the technical debt or risk>
                - **[SUGGESTION]** <File Path>:<Line> - <Readability or style enhancement>

                ### Final Verdict
                - **Verdict**: [APPROVED | REQUIRES_CORRECTION]
                *(Set to REQUIRES_CORRECTION if at least one CRITICAL issue is found, otherwise APPROVED)*

                User context: $ARGUMENTS
                """,
            FilePath = "<builtin>",
            WhenToUse = "Use ONLY when the user wants a code review. This skill is designed to analyze code changes, identify critical flaws, and provide structured feedback to ensure code quality and robustness.",
            Source = "builtin"
        },
        new SkillDef
        {
            Name = "dynamo-script",
            Description = "Mandatory playbook to Atuodesk Dynamo Visual Programming, node hierarchies and data flow scenarios using Dynamo library primers and forum practices",
            Triggers = new List<string> { "/dynamo-script", "/script", "/dynamo" },
            Tools = new List<string> 
            { 
            "DynamoReadGraph","DynamoListNodes", "DynamoSearchLibrary", 
            "DynamoRunGraph", "DynamoGetWarnings", "DynamoWriteNode", 
            "DynamoEditNode", "DynamoGetNodeConnections", 
            "WebFetch", "WebSearch" , "Read", "Grep", "Glob"
            },
            Prompt = """
                Follow these best practices when creating, editing, modifying or optimising Dynamo scripts using the Dynamo Visual Programming for Revit 2026. Always adhere to these guidelines to ensure robust, efficient, and maintainable scripts:
                
                ## Domain Restrictions
                - You MUST strictly prioritize and scope your web queries toward:
                * site:forum.dynamobim.com (Dynamo specific logic and nodes)
                * site:forums.autodesk.com (Official Autodesk developer community)
                * GitHub repositories containing official Autodesk/Dynamo samples.

                ## Code Extraction Rules
                - NEVER copy-paste web code blindly. Use search results ONLY to verify method signatures, enum naming changes, and structural workflows.

                ## Node Flow & Layout Discipline
                - Always structure your solutions logically from Left to Right:
                * INPUTS (Code Blocks, File Paths, Select Model Elements, Categories)
                * ACTIONS (Element.GetParameterValueByName, Geometry operations, Filter Masking)
                * OUTPUTS (Watch Nodes, Data Export, Element.SetParameterByName)
                - Explicitly name the exact nodes the user needs to place on their canvas (e.g., Use 'List.FilterByBoolMask', NOT just 'filter the list').

                ## List Control & Lacing Rules
                - You MUST explicitly dictate the 'Lacing' and 'List@Level' settings for critical nodes to prevent empty lists or structural graph crashes:
                * Tell the user when to use List Levels (e.g., Set 'list' input to @L2).
                * Specify the Lacing configuration (Shortest, Longest, or Cross Product) when matching mismatched datasets (e.g., Matching 5 Views with 20 Elements requires Cross Product).

                ## Package & Custom Node Restrictions
                - Prioritize Out-Of-The-Box (OOTB) standard Dynamo nodes whenever possible.
                - If a task requires a custom package, explicitly name the authoritative package and node (e.g., "Use 'Revit.Elements.Views' from Clockwork package"). Do NOT invent fictional packages.

                ## The Python Bridge & Boundaries
                - You are strictly PROHIBITED from writing massive Revit API / IronPython scripts inside this skill.
                - If a workflow cannot be achieved using standard nodes or simple DesignScript syntax inside a Code Block, STOP. 
                - Instruct the Orchestrator to instantiate a 'Python Script' node and hand over the execution to the 'revit-expert' sub-agent.
                - Code Blocks (DesignScript) should ONLY be recommended for simple variable declarations, direct properties (e.g., 'x.Length;'), or basic list flattenings. 
                - NEVER use Code Blocks to write complex custom functions or simulated programming logic. If it requires logic, shift it to 'revit-api'.

                User request: $ARGUMENTS
                """,
            FilePath = "<builtin>",
            WhenToUse = "Use ONLY when the user or task requires to Dynamo graph creation, modification, optimization, and debugging, including element manipulation, parameter access, geometry processing, and performance improvements. This skill is designed to provide precise node-level instructions and best practices for Dynamo scripting within the Revit context.",
            Source = "builtin"
        }, 
        new SkillDef
        {
            Name = "revit-api",
            Description = "Mandatory playbook to create, optimize, refactor, or debug Dynamo Python nodes. Use for advanced Revit/Dynamo API automation, heavy geometry transactions, multi-library conversion, performance profiling, and fixing graph warnings.",
            Triggers = new List<string> { "/Dynamo-Python", "/Dynamo-code" },
            Tools = new List<string> 
            { 
            "RevitApiDocsFetch", "WebFetch", "WebSearch", "Read", "Grep", "Glob"
            },
            Prompt = """
                Follow these best practices when creating, editing, modifying or optimising Dynamo Python scripts using the Autodesk Revit API for Revit 2026. Always adhere to these guidelines to ensure robust, efficient, and maintainable code:
                
                ## Domain Restrictions
                - You MUST strictly prioritize and scope your web queries toward:
                * site:thebuildingcoder.typepad.com (Jeremy Tammik's gold standard Revit API guide)
                * site:forums.autodesk.com (Official Autodesk developer community)
                * site:forum.voltadynabim.blogspot.com (Python guru for Dynamo Python script)
                * GitHub repositories containing official Autodesk/Dynamo/Python samples.

                ## Version Awareness
                - Always append the explicit target Revit/Dynamo version (e.g., 'Revit 2024 Units API', 'Revit 2026') to your searches to capture breaking API changes.

                ## Code Extraction Rules
                - NEVER copy-paste web code blindly. Use search results ONLY to verify method signatures, enum naming changes, and structural workflows.
                
                ## Transaction Management
                - Always wrap database modifications in a Transaction: using (Transaction tx = new Transaction(doc, "name")) { tx.Start(); ... tx.Commit(); }
                - Use SubTransaction for nested or reversible operations within a main transaction
                - Roll back on failure: catch exceptions and call tx.RollBack() to leave the document clean
                - Group logically related changes into a single transaction for performance
                - Do NOT use transaction in read-only contexts (failure analysis, parameter reads, element enumeration)
                - When iterating over massive element collections from FilteredElementCollector, explicitly close/dispose transient geometry objects or use SubTransactions where native memory leak is a risk.
                - Ensure all transaction boundaries are closed explicitly; a stuck transaction will lock the user's Revit UI completely.

                ## FilteredElementCollector Patterns
                - Use OfClass(typeof(...)) for filtering by API class; combine with WhereElementIsNotElementType() for instance-only results
                - Use OfCategory(BuiltInCategory.OST_...) for category-based filtering (faster than OfClass for built-in categories)
                - Chain WherePassThrough filters for custom logic: collector.WherePassThrough(new ElementMulticlassFilter(filterList))
                - Use ToElements() or ToElementIds() only after all filtering is done — avoid materializing early
                - Cache collectors that are reused: collect once, iterate multiple times

                ## Dynamo ↔ Revit Complex Geometry Conversions
                - Dynamo uses ProtoGeometry (Autodesk.DesignScript.Geometry), Revit uses native API geometry (XYZ, Curve, Solid, etc.)
                - Conversion: Dynamo Point → Revit XYZ: XYZ(Point.X, Point.Y, Point.Z)
                - Conversion: Dynamo Surface → Revit Face: use ExportToBrep() or geometry extraction via GetGeometryObjectFromReference
                - Conversion: Revit Curve → Dynamo Curve: use CurveByPoints or imported geometry converters
                - Always dispose ProtoGeometry objects: wrap in using() or call .Dispose() to avoid memory leaks
                - Watch for unit differences: Revit internal units are feet, Dynamo typically uses project units
                - Shapely ↔ Dynamo geometry: Surface → Polygon, Curve → LineString, Point → Point
                - Handle multi-surface/hole geometries (surfaces with interior loops → polygons with holes)
                - Curve connectivity: join disconnected curve segments into closed loops before conversion
                - Coordinate system alignment between Dynamo (model space) and Shapely (2D/3D)
                - Performance: batch-process geometry sets; avoid per-element AddReference calls
                - Dynamo ProtoGeometry → Shapely → NumPy/SciPy (for computational geometry)
                - Maintain precision: watch for floating-point drift across library boundaries
                - Document conversion assumptions (units, coordinate hand, tolerance)

                ## Element Parameter Access
                - Use get_Parameter(BuiltInParameter) for built-in parameters — avoids string lookup overhead
                - Use LookupParameter("ParameterName") for shared/project parameters
                - For parameter modification: parameter.Set(value) must happen inside a transaction
                - Check parameter.StorageType before reading to determine the value type (Integer, Double, String, ElementId)

                ## Revit Version Compatibility
                - Revit 2022+: ForgeTypeId-based unit system replaces DisplayUnitType — use UnitUtils.ConvertFromInternalUnits
                - Revit 2024+: Enhanced SectionBox, Topography API changes
                - Revit 2025-2026+: New Element APIs, performance improvements in geometry extraction
                - Use #if REVIT202x conditional compilation for version-specific code
                
                ## Revit/Dynamo API Essentials
                - Use proper imports (clr, Autodesk.DesignScript.Geometry, RevitAPI, RevitServices)
                - Handle transactions properly via DocumentManager
                - Use FilteredElementCollector with proper type casting
                - Differentiate Dynamo vs Revit geometry
                - Include proper error handling
                - Use doc, uidoc, UnwrapElement conventions

                ## Code Optimization & Refactoring
                - Profile bottlenecks first: identify nested loops, redundant type conversions, repeated API calls
                - Extract helper functions for reusable geometry logic (e.g., curve-to-loop assembly)
                - Use list comprehensions and generator expressions for large geometry collections
                - Cache expensive conversions (surface tessellation, polygon construction)
                - Transaction batching: group related geometry modifications into single transactions

                ## Authoritative Reference Framework
                When designing algorithms, optimizing bottlenecks, or structure handling, you must align with and actively query:
                - PEP 8 (Official Style Guide) & PEP 20 (Zen of Python): For strict idiomatic code.
                - Python Software Foundation (PSF) Speed Wiki: For native execution speed and lower time complexity O(n).
                - Real Python & Effective Python Guidelines: For modern memory-efficient data structures (Python 3.10+).

                ## Core Execution & Performance Mandates
                - Avoid nested loops at all costs; leverage O(1) dictionary lookups or set intersections.
                - Memory Efficiency: For sorting or processing large abstract datasets, use generator expressions or custom iterators instead of massive list comprehensions to prevent memory bloating.
                - Use local variables inside performance-critical functions (Python scopes local variables faster than globals).
                - Utilize built-in high-performance structures like `collections.deque` for queues or `collections.defaultdict` for grouping.

                User request: $ARGUMENTS
                """,
            FilePath = "<builtin>",
            WhenToUse = "Use ONLY when the user or task requires creating, modifying, optimising, deleting, or querying all Revit operation (e.g., elements, parameters, types, or views) inside a Dynamo Python node",
            Source = "builtin"
        },
        new SkillDef
        {
            Name = "python-code",
            Description = "Mandatory playbook to general Python coding for writing, refactoring, optimizing, and debugging code outside Dynamo context.",
            Triggers = new List<string> { "/code","/optimize-list", "/sort-data", "/parse-json", "/math-calculation", "/string-manipulation", "/pure-python" },
            Tools = new List<string> 
            { 
            "PythonInterpreter", "WebSearch", "WebFetch" , "Read","Grep", "Glob"
            },
            Prompt = """
                Follow these best practices when creating, editing, modifying or optimising Python code. Always adhere to these guidelines to ensure robust, efficient, and maintainable code:

                ## Domain Restrictions
                - You MUST strictly prioritize and scope your web queries toward:
                * site:realpython.com (Comprehensive Python tutorials and best practices)
                * site:stackoverflow.com (Community Q&A for specific coding problems)
                * site:docs.python.org (Official Python documentation for language features and standard library)
                * GitHub repositories containing official Python samples and libraries.

                ## CRITICAL BOUNDARY
                - This skill is strictly for pure Python data processing, algorithmic refactoring, and general coding tasks outside the Revit/Dynamo context. If there is ANY possibility that the input contains Revit-specific objects or requires Revit API calls, you MUST defer to 'revit-api' to ensure proper transaction management and API usage.
                
                ## Code Structure & Modularity
                - Keep functions small, focused on a single responsibility
                - Use descriptive names for variables, functions, and classes
                - Prefer composition over inheritance
                - Separate concerns: data processing, I/O, and presentation logic should be distinct
                - Use type hints for function signatures to improve readability and catch type errors early
                - Write high-performance, clean, and idiomatic Python 3.x code (PEP 8 compliant).
                - Focus heavily on data structures, algorithmic complexity (O(n)), memory efficiency, and robust type checking.
                - Perfect for handling heavy lifting like JSON/XML parsing, regex operations, dictionary mappings, and custom mathematical/geometric matrix calculations.
                
                ## Authoritative Reference Framework
                When designing algorithms, optimizing bottlenecks, or structure handling, you must align with and actively query:
                - PEP 8 (Official Style Guide) & PEP 20 (Zen of Python): For strict idiomatic code.
                - Python Software Foundation (PSF) Speed Wiki: For native execution speed and lower time complexity O(n).
                - Real Python & Effective Python Guidelines: For modern memory-efficient data structures (Python 3.10+).

                ## Core Execution & Performance Mandates
                - Avoid nested loops at all costs; leverage O(1) dictionary lookups or set intersections.
                - Memory Efficiency: For sorting or processing large abstract datasets, use generator expressions or custom iterators instead of massive list comprehensions to prevent memory bloating.
                - Use local variables inside performance-critical functions (Python scopes local variables faster than globals).
                - Utilize built-in high-performance structures like `collections.deque` for queues or `collections.defaultdict` for grouping.

                ## Performance Optimization
                - Use list comprehensions and generator expressions instead of manual loops where clarity is not sacrificed
                - Cache expensive function results using functools.lru_cache or manual caching
                - Prefer built-in functions and standard library over custom implementations (map, filter, itertools, collections)
                - Avoid repeated attribute lookups: bind frequently accessed methods to local variables
                - Use collections.deque for efficient queue operations, defaultdict for grouping
                - Use generator expressions instead of massive list comprehensions for large datasets.

                ## Error Handling Patterns
                - Catch specific exceptions, not bare except: blocks — know what can fail and how
                - Use try/except/else/finally correctly: else runs when no exception, finally always runs
                - Fail fast for invalid inputs: validate parameters at function entry, raise ValueError or TypeError immediately
                - Graceful degradation: when a non-critical operation fails, log and continue rather than crashing
                - Avoid swallowing exceptions silently — at minimum log the error with context

                ## Input Validation & Edge Cases
                - Check for None, empty collections, and boundary values before processing
                - Handle both scalar and iterable inputs where appropriate
                - Use structural pattern matching (Python 3.10+) for clean type-based dispatch
                - For numerical code, handle division by zero, overflow, and precision loss explicitly
                - Document assumptions about input format, units, and ranges

                User request: $ARGUMENTS
                """,
            FilePath = "<builtin>",
            WhenToUse = "Use ONLY when the user or task requires writing, general Python processing, refactoring algorithms, or sorting data, or optimizing general Python code (non-Dynamo / non-Revit context).",
            Source = "builtin"
        }
    };
}    