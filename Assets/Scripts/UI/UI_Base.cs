using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.UIElements;

public abstract class UI_Base : MonoBehaviour
{
    /// <summary>
    /// ������ object (GameObject, Text, Image �� ��Ī) �ڷᱸ��
    /// </summary>
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    /// <summary>
    /// initiate �Լ�
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// T Ÿ�� objects dic�� ����
    /// </summary>
    /// <typeparam name="T"> �ش� Ÿ�� </typeparam>
    /// <param name="type"> �ش� Ÿ�� ���� ���� enum </param>
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objs = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objs);

        for (int i = 0; i<names.Length; i++)
        {
            objs[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objs[i] == null)
            {
                Debug.Log($"Failed to Bind : {names[i]} on {gameObject.name}");
            }
        }
    }
    /// <summary>
    /// �ش� game object�� �̺�Ʈ �Ҵ�
    /// </summary>
    /// <param name="action">�Ҵ��� �̺�Ʈ</param>
    /// <param name="type">�̺�Ʈ �߻� ����</param>
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }

    /// <summary>
    /// bind�� object���� ���ϴ� object ���
    /// </summary>
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }
    #region Get_Override
    protected GameObject GetGameObject(int idx) => Get<GameObject>(idx);
    protected TMP_Text GetText(int idx) => Get<TMP_Text>(idx);

    protected Image GetImage(int idx) => Get<Image>(idx);
    protected Button GetButton(int idx) => Get<Button>(idx);
    #endregion Get_Override

   

    /// <summary> �ݺ������� ����ϱ� ���� index ��� event �Ҵ� </summary>
    public static void BindEvent(GameObject go, Action<PointerEventData, object> action, object pivot, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_PivotEventHandler evt = Util.GetOrAddComponent<UI_PivotEventHandler>(go);
        evt.Pivot = pivot;

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
        }
    }
}