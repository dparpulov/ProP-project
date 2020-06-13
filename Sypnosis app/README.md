![alt text](Pictures/sypnosisLOGO.png)
ProP Group 31
=============
>Edited: 08/05/2018 12:43

ONFest, which is part of the company Sypnosis, organizes music festivals. Due the increase of visitors to events, it has become more difficult to manage. This project's goal is to develop a solution to make events more manageable. 

http://i388898.hera.fhict.nl/ONFest/

Gantt Chart
-----------
```mermaid
gantt
    title ProP Group 31 Gantt Chart
    dateFormat  YYYY-MM-DD

    section Research
    QR Code           :done, 2018-02-13, 2d

    section Project Plan
    Project Plan v1     :done, pp1, 2018-02-13, 7d
    Project Plan v2     :done, after pp1, 7d

    section Setup Document
    Setup Document v1      :done, sd1, 2018-02-13, 7d
    Setup Document v2      :done, after sd1, 7d

    section Process Report
    Process Report      :active, pr1, 2018-02-13, 112d

    section Database Design
    Database Design v1      :done, dd1, 2018-02-27, 7d
    Database Design v2      :done, dd2, after dd1, 7d
    Database Design v3      :done, dd3, after dd2, 7d
    Database Design v4      :done, dd4, after dd3, 7d
    Database Design v5      :done, dd5, after dd4, 7d
    Database Design v6      :done, dd6, after dd5, 7d

    section Website
    Front-end     :done, web1, 2018-03-06, 14d
    Back-end      :active, web2, after web1, 80d

    section Application
    Application GUI1  :done, app1, 2018-03-15, 14d
    Application GUI2  :active, app2, 2018-04-21,25d
    Application Code :active, app3, 2018-04-21, 50d

    section Extra features
    Extra features   :active, ef,2018-06-05, 7d

    section Debug
    Debug website     :active, dw, 2018-06-11, 3d
    Debug application :active, da, after dw, 3d

    section Presentation
    Presentation     :active, ppt, 2018-06-18, 7d
    

```

Repository structure
----------
- Documentation
	- Database Design
	- Meetings
		- Agendas
		- Minutes
	- Process Report
	- Product Design
	- Project Plan
	- Setup Document
	- ToDo list
- Pictures
- Resources

Standards
---------
Documentation are to be named as following:
	`Document Name.vx`

Agendas and minutes are to be prefixed with the date as follows:
	`YYYYMMDD_agenda/minute.md`
	
Agendas, minutes and README files are of filetype Markdown(.md). Markdown files are easier to read in GitLab. To view these files outside of GitLab, simply open them with a text editor.