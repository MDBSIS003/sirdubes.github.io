# Display layout correction — version 1.0.1

Windows display scale and screen resolution can differ between a Mac running Windows and another PC. A Retina display often uses a higher scale. In the earlier code, text could grow while the sidebar and cards retained fixed pixel sizes. This caused FUTURE/PATH to wrap into additional lines and navigation text to be clipped. The user confirmed that the Windows 10 installation uses 200% display scaling. The Windows 11 scale was not supplied.

## Use the corrected Visual Studio project

1. Close Future Path and stop debugging in Visual Studio.
2. Extract the entire corrected ZIP into a new folder, separate from the previous project.
3. Open **FuturePath.sln from this new folder**, then choose **Debug → Start Debugging**.
4. Keep the Windows display scale that is comfortable for your screen. The app declares a 96-DPI design baseline, scales its forms and pages together, fits cards to the viewport and sizes sidebar buttons for their text. On a very wide display, cards have a reading-width limit of 1100 logical pixels.

The app connects to the same `(localdb)\MSSQLLocalDB` / `FuturePathStudentAdvisingSystem` database under your current Windows account. Opening a new source folder does not move, recreate or clear that database. A different Windows installation/account has its own LocalDB instance; databases do not synchronise between machines. If the app already works on this Windows installation, no database setup scripts need to be repeated for this display correction.

## Temporary workaround for the previous build

Close the app. Open Windows **Settings → System → Display → Scale and layout**. If text remains readable for you, temporarily select **100%** under **Change the size of text, apps, and other items**, then reopen the app. This changes the size of all Windows apps. With the corrected build you can return to your preferred/recommended scale. Display resolution and scale can still produce different physical window sizes on different screens.

## What changed

- AppForm and AppPage share the explicit 96-DPI baseline, with layout suspended until construction completes.
- Dynamically opened learner steps, reports and staff pages use the same scaling mode. Retained steps remain intact when navigating Back/Next.
- Sidebar width follows the current monitor DPI; its labels and buttons fit the available width and the menu can scroll.
- Header and learner footer calculate their heights from content; cards grow/shrink with the viewport and text reflows inside them.
- The initial main window is constrained to the current monitor working area.

Both solutions compile, including the Windows layout regression cases. The Linux workspace cannot run Windows Forms. Actual Windows 10/11 checks at 100%, 125%, 150% and 200%, plus movement between displays with different scale settings, remain required. See docs/Validation.md and TC37 in docs/Test-Cases.md.
