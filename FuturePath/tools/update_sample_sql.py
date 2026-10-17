"""After extract_sample_fixtures.py and tests --emit-sample-sql, replace only the generated seed block."""
from pathlib import Path
root=Path(__file__).resolve().parents[1]
p=root/'Database/03_InsertSampleData.sql'
s=p.read_text();start=s.index('-- BEGIN GENERATED ASSESSMENTS');end=s.index('-- END GENERATED ASSESSMENTS',start)
s=s[:start]+'-- BEGIN GENERATED ASSESSMENTS\n'+(root/'tools/sample-assessments.sql').read_text()+s[end:]
p.write_text(s)
for p in (root/'Database').glob('*.sql'):(root/'FuturePathStudentAdvisingSystem/Database'/p.name).write_bytes(p.read_bytes())
