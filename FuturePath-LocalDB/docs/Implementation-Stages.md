# Implementation stages

| Stage | Changed | Tested | Remaining boundary |
|---|---|---|---|
|1 Inspect | Preserved separate reference copy/branch; inspected all projects, schema/data, docs, UI, security, reports and test files; inventoried SQLite references | Original 58 tests executed successfully before changes | No original Windows GUI execution |
|2 Data migration | DatabaseConnection/SqlServerStore; SQL Server types/identities/date queries; explicit safe scripts; generated outcomes from unchanged inputs; atomic assessment/audit; SQL Server backup | Full compile, SQL grammar parsing, connection-config validation, provider removal checks | Real LocalDB setup/CRUD/restart/rollback/backup tests need Windows |
|3 Single-window integration | Retained learner panels, validated Next/Back, progress/Finish; embedded report and staff editors; clear deactivation/unsaved confirmations | Shared validation/engine tests pass; four Windows UI tests compile | Manual Windows navigation/DPI/printing not executed |
|4 Simplify | Conventional service constructors, named weights/thresholds, explicit OR grouping, expanded readable source; preserved existing architecture/features | Required outcomes and boundary tests pass | Team should rehearse the walkthrough and review its approved design |
|5 Verify/document/package | README/setup/dictionary/traceability/test cases/walkthrough/presentation/dependency/migration docs; updated diagrams/Word; app-only solution and launcher | See Validation.md and actual logs/TRX for exact final results | Do not call this fully demonstrated until the Windows cases run |

No framework downgrade, new UI framework, external report engine, online API or AI feature was added. Original uploads and earlier integration are retained separately. No real SQLite learner database was attached for import; existing files are untouched.
