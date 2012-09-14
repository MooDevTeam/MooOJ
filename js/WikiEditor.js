function decorateText(txtWiki, prefix, suffix, hint) {
    var start = txtWiki.selectionStart;
    var end = txtWiki.selectionEnd;
    if (start == end) {
        txtWiki.value = txtWiki.value.substring(0, start) + prefix + hint + suffix + txtWiki.value.substring(end);
        txtWiki.selectionStart = start + prefix.length;
        txtWiki.selectionEnd = start + prefix.length + hint.length;
    } else {
        txtWiki.value = txtWiki.value.substring(0, start) + prefix + txtWiki.value.substring(start, end) + suffix + txtWiki.value.substring(end);
        txtWiki.selectionStart = start;
        txtWiki.selectionEnd = end + prefix.length + suffix.length;
    }
}
function toBold(clientID) {
    decorateText(document.getElementById(clientID), "*", "*", "在此输入需要加粗的内容");
}
function toItalic(clientID) {
    decorateText(document.getElementById(clientID), "_", "_", "在此输入需要倾斜的内容");
}
function toUnderline(clientID) {
    decorateText(document.getElementById(clientID), "+", "+", "在此输入需要加下划线的内容");
}
function asMath(clientID) {
    decorateText(document.getElementById(clientID), "<math>", "</math>", "在此输入LaTeX数学公式");
}
function changeColor(clientID, selectElement) {
    var color = selectElement.options[selectElement.selectedIndex].value;
    var colorName = selectElement.options[selectElement.selectedIndex].text;
    if (color != "") {
        decorateText(document.getElementById(clientID), "<color:" + color + ">", "</color>", "在此输入需要显示为" + colorName + "的内容");
        selectElement.selectedIndex = 0;
    }
}
function asTitle(clientID, selectElement) {
    var titleLevel = selectElement.options[selectElement.selectedIndex].value;
    var levelName = selectElement.options[selectElement.selectedIndex].text;
    if (titleLevel != "") {
        decorateText(document.getElementById(clientID), "\n" + titleLevel + " ", "\n", "在此输入需要作为" + levelName + "的内容");
        selectElement.selectedIndex = 0;
    }
}
function asSourceCode(clientID, selectElement) {
    var language = selectElement.options[selectElement.selectedIndex].value;
    var languageName = selectElement.options[selectElement.selectedIndex].text;
    if (language != "") {
        decorateText(document.getElementById(clientID), "\n{code:" + language + "}\n", "\n{code:" + language + "}\n", "在此输入" + languageName + "源码");
        selectElement.selectedIndex = 0;
    }
}