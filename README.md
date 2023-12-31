# ⏩ 유니티 커스텀 단축키
> 기본으로 제공하는 단축키 외에도 필요한 기능을 개발하여 등록할 수 있다.
## 🔍 단축키 생성방법
+ Attribute인 `MenuItem`에 단축키를 추가하는 방식이다.
+ 단축키를 눌렀을 때, 실행될 함수는 **static**으로 선언되어야 한다.
+ 키 조합 매뉴얼은 아래와 같다.
    - 조합키
        + `%` : Ctrl or Cmd(Mac OS)
        + `^` : Ctrl
        + `#` : Shift
        + `&` : Alt
        + `_` : 조합키가 필요하지 않은 경우
    - 특수문자
        + `LEFT`, `RIGHT`, `UP`, `DOWN`, `F1부터 F12`, `HOME`, `END`, `PGUP`, `PGDN`, `INS`, `DEL`, `TAB`, `SPACE`
    - 예시
        + **Shift+Alt+G** 단축키를 가진 메뉴를 만들려면 `"MyMenu/Do Something #&g"`를 사용하고, G키만 사용하려면 `"MyMenu/Do Something _g"`를 사용한다.
## 🗒️ 구현 함수
> 이 함수들은 모두 **Undo**, **Redo**를 지원한다.
+ `Group` `<Ctrl+G>` : 새 오브젝트를 생성하고, 선택된 오브젝트들을 자식으로 설정한다. 부모가 다르면 각각 그룹화한다. 위치는 평균값으로 지정하고, RectTransform을 지원한다.
+ `Ungroup` `<Ctrl+Shift+G>` : 선택된 오브젝트들을 제거하고, 자식들을 밖으로 빼낸다.
