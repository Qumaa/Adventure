using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MyEditorWindow : EditorWindow
{
    const string SCRIPT_SEARCH_FILTER = "a:assets t:script";

    ListView _leftPane;
    ScrollView _rightPane;

    [SerializeField] int _lastSelectedIndex;

    [MenuItem("Tools/My Editor")]
    public static void CreateWindow()
    {
        EditorWindow window = GetWindow<MyEditorWindow>();
        window.titleContent = new GUIContent("Script browser");

        window.minSize = new Vector2(500, 240);
    }


    private void CreateGUI() => CreateView();


    private void CreateView()
    {
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        rootVisualElement.Add(splitView);

        _leftPane = GetListView(GetAllScripts(SCRIPT_SEARCH_FILTER));
        splitView.Add(_leftPane);
        _leftPane.onSelectionChange += OnSelectionChangeHandle;

        _rightPane = GetContentView();
        splitView.Add(_rightPane);

        _leftPane.selectedIndex = _lastSelectedIndex;        
    }


    private ListView GetListView(MonoScript[] allObjects) => new ListView
    {
        makeItem = () => new Label(),
        bindItem = (item, index) => (item as Label).text = allObjects[index].name,
        itemsSource = allObjects
    };

    private ScrollView GetContentView() => new ScrollView(ScrollViewMode.VerticalAndHorizontal);

    private MonoScript[] GetAllScripts(string searchFilter)
    {
        var allScriptsGuids = AssetDatabase.FindAssets(searchFilter);
        var allScripts = new MonoScript[allScriptsGuids.Length];

        for (int i = 0; i < allScriptsGuids.Length; i++)
            allScripts[i] = AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(allScriptsGuids[i]));

        return allScripts;
    }

    private void OnSelectionChangeHandle(IEnumerable<object> selectedItems)
    {
        _rightPane.Clear();

        var currentScript = selectedItems.First() as MonoScript;
        if (currentScript == null)
            return;

        _rightPane.Add(new Label(currentScript.name));
        _rightPane.Add(SeparationLine());
        _rightPane.Add(new Label(currentScript.ToString()));

        _lastSelectedIndex = _leftPane.selectedIndex;
    }

    private VisualElement SeparationLine()
    {
        VisualElement line = new VisualElement();

        line.style.height = 1;
        line.style.backgroundColor = Color.black;

        return line;
    }
}
