Projektas Dagra

Sistemos paskirits- palengvinti įmonių administratorių tvarkaraščių sudarymą ir įmonių darbuotojams palengvinti tvarkaraščių pasiekiamumą.

Veikimo principas:
Įmonės atstovas, atsakingas už darbuotojų tvarkaraščius, sukuria admino paskyrą ir įmonę. Toliau administratorius gali kurti tvarkaraščius, darbus ir darbų priskyrimus darbuotojams. Kad darbuotojai būtų matomi sistemoje jie taip pat turi prisiregistruoti prie sistemos. Svečiai, gavus įmonės administratoriaus leidimą, gali stebėti tvarkaraščius, tačiua negali nieko keisti.

Dagra turi 4 esybes:
- Company (įmonė)
- Schedule (tvarkaraštis)
- Job (darbas)
- Assignment (darbo priskyrimas)

Pavyzdžiui: įmonė/restoranas turi virtuvės ir padavėjų tvarkaraščius (schedules), virtuvė turi karštų patiekalų ir desertų darbus (jobs), 
ir karštų patiekalų kepėjai dirba dvejomis pamainomis (assignments)

/api/company/{companyId}/schedules/{scheudle_id}/jobs/{job_id}/assignment/{assignment_id}
