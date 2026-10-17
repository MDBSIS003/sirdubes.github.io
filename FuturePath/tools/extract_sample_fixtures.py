"""Extract noncredential sample data from the canonical SQL seed for business-rule tests."""
import json,re
from pathlib import Path
root=Path(__file__).resolve().parents[1]
sql=(root/'Database/03_InsertSampleData.sql').read_text()
def rows(table):
    m=re.search(r'INSERT INTO '+table+r'\s*\((.*?)\)\s*VALUES\s*(.*?)SET IDENTITY_INSERT '+table+r' OFF;',sql,re.S)
    cols=[x.strip() for x in m[1].split(',')]
    # Only strip SQL comments outside strings. Tokenise quoted strings with doubled-quote escapes.
    tokens=re.findall(r"'(?:[^']|'')*'|--[^\n]*|\bNULL\b|-?\d+(?:\.\d+)?|[(),;]",m[2])
    out=[]; row=None
    for t in tokens:
        if t.startswith('--'):continue
        if t=='(':row=[]
        elif t==')':out.append(dict(zip(cols,row)));row=None
        elif row is not None and t not in ',;':
            row.append(t[1:-1].replace("''", "'") if t.startswith("'") else None if t=='NULL' else float(t) if '.' in t else int(t))
    return out
subjects={r['SubjectID']:r['SubjectName'] for r in rows('Subject')}
faculties={r['FacultyID']:r['FacultyName'] for r in rows('Faculty')}
degrees=rows('DegreeProgramme')
for r in degrees:r['FacultyName']=faculties[r['FacultyID']];r['IsActive']=bool(r['IsActive'])
rules=rows('ProgrammeRequirement')
for r in rules:
 r['SubjectName']=subjects[r['SubjectID']];r['IsMandatory']=bool(r['IsMandatory'])
 r['AlternativeGroup']='Sample numeracy choice' if r['AdvisoryNote'].startswith('Sample alternative:') else None
questions=rows('InterestQuestion');marks=rows('LearnerSubject');answers=rows('LearnerInterestResponse')
sessions=rows('AdvisorySession')
for r in sessions:
 r['SessionDate']=r['SessionDate'][:8]+' '+r['SessionDate'][9:] # overwritten with ISO below
 date=r['SessionDate'];r['SessionDate']=date[:4]+'-'+date[4:6]+'-'+date[6:8]+'T'+date[9:]
 r['SubjectsComplete']=r['SessionID']!=9;r['FundingRequested']=r['SessionID'] in [1,4,8];r['IsFinalized']=True
 r['Marks']=[{'SubjectID':m['SubjectID'],'SubjectName':subjects[m['SubjectID']],'Mark':m['Mark']} for m in marks if m['SessionID']==r['SessionID']]
 r['Responses']=[{'QuestionID':a['QuestionID'],'Score':a['Score']} for a in answers if a['SessionID']==r['SessionID']]
 # Starter added two questions but omitted sample responses. For demonstration data, copy same-category answers.
 if r['Responses']:
  scores={a['QuestionID']:a['Score'] for a in r['Responses']}
  for q,source in [(9,1),(10,2)]:
   if q not in scores:r['Responses'].append({'QuestionID':q,'Score':scores[source]})
data={'Degrees':degrees,'Requirements':rules,'Questions':questions,'Mappings':rows('DegreeInterestMapping'),'Sessions':sessions}
(root/'FuturePath.Tests/sample-fixtures.json').write_text(json.dumps(data,indent=2)+'\n')
print('Extracted sample fixtures: 10 degrees, 10 sessions; no passwords or account contact data.')
