# Display layout in the course edition

The reported Windows 10 display setting was **200%**. The previous fixed sidebar/card layout could wrap FUTURE PATH into several short lines and leave unused space. This edition replaces that sidebar with the group's seven-screen flow and standard Designer-backed forms.

The application declares PerMonitorV2 DPI awareness, uses a 96-DPI design baseline and Segoe UI, docks the page through TableLayoutPanel, keeps Back/Next in a separate footer and scrolls the content area. Staff forms clamp their size to the screen's working area. The app starts maximised. Do not change the Windows display scale solely to make this version usable.

Extract to a new folder, open FuturePath.sln and press F5. It uses the same LocalDB database under the same Windows account. Test Windows 10 and 11 at 100%, 125%, 150%, 175% and 200%, including Back/Next, questionnaire wrapping, report filters and staff editors. Actual DPI/monitor changes cannot be executed in the Linux build environment; these checks remain pending.
