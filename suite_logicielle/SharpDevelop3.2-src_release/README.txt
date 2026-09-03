INSTALLATION

- Copy the dlls from the Beans directory to the Beans directory of SharpWComp (>=2.4.0)
- Copy the AADesigner directory in SharpWcomp's installation directory (optional, for AADesignerSolution default path setup)


EASY LAUNCH

Open the AADesignerSolution/AA.sln solution with SharpWComp, then open the three files in the solution.
To initialize it, click in all textboxes of the UI file, from the one at the top to the one at the bottom.
Verify that the path for the AA repository is correct, clicking on it should display the list of AAs in it in the list.
The AA system should be working if the proxy beans in the UI and the weaver containers have a green border, meaning that the configured Uri are correct.



COMPLICATED LAUNCH (not required if the easy launch is used)

The traditional way of launching AAs is to manually load the WCC files in containers and configure all textboxes. It is explained below.

- Start SharpWComp
- Open three WComp containers
- In the first, load the AAReader.wcc, using WComp.NET -> Import menu
- In the second, load the weaver.wcc
- Bind the second and the empty third, using WComp.NET -> Bind to UPnP Device

Comments :
- WComp_NetAppli_ContainerX is the structural (or control) interface of the container X
- WComp_NetProbe_ContainerX is the functional interface of the container X
- Use a UPnP control point, like Intel's Device Spy, to find the URL of the two UPnP-bound containers.
(You can identify the UPnP device with the name of the file of the container, appearing as the tab name)

TODO: 
1> Copy the URL of the structural (or control) interface of the weaver (WComp_NetAppli_ContainerX) into the WeaverControlURL textbox of the first container 
Validate it by clicking outside the textbox FIRST and THEN click on the Init button.

2> Copy the URL of the functional interface of the weaver (WComp_NetProbe_ContainerX) into the WeaverFunctionalInterface textbox of the first container
Validate it by clicking outside the textbox

3> Copy the URL of the structural (or control) interface of the application (WComp_NetAppli_ContainerX) into the ApplicationControlURL textbox of the first container
Validate it by clicking outside the textbox

4> Copy the path of the directory where your aspects of assembly files are (see AADesigner installation directory)
Validate it by clicking outside the textbox


- Then you can (slowly) select AAs in the list, the weaver container (2nd) should be modified, and you should have a feedback message on the label of the first container.

If selecting an AA by clicking on its name does not displays its definition in the large textbox, or if the list is not initialized when you set the path of the AA directory, there is a communication problem between the weaver and its UI (containers 2 and 1).

If selecting an AA has no effect, and unchecks the selection checkbox of other AAs, you have a communication problem inside the weaver, meaning the "click on Init button" was unsuccessful.

If the weaver crashed for some reason, like a setup issue, you can reload the assembly weaver.wcc at least, and AAReader.wcc if needed too. Be careful that the URL of the weaver will change on assembly reload, because its functional interface is modified.

