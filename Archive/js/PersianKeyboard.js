﻿var defaultSettings = "fa"; (function (e) { e.fn.persiaNumber = function (t) { function r(e, t) { e.find("*").andSelf().contents().each(function () { if (this.nodeType === 3 && this.parentNode.localName != "style" && this.parentNode.localName != "script") { this.nodeValue = this.nodeValue.replace(this.nodeValue.match(/[0-9]*\.[0-9]+/), function (e) { return e.replace(/\./, ",") }); this.nodeValue = this.nodeValue.replace(/\d/g, function (e) { return String.fromCharCode(e.charCodeAt(0) + t) }) } }) } if (typeof t == "string" && t.length > 0) defaultSettings = t; var n = 1728; if (t == "ar") { n = 1584 } window.persiaNumberedDOM = this; r(this, n); e(document).ajaxComplete(function () { var e = window.persiaNumberedDOM; r(e, n) }) } })(jQuery); origParseInt = parseInt; parseInt = function (e) { e = e && e.toString().replace(/[\u06F0\u06F1\u06F2\u06F3\u06F4\u06F5\u06F6\u06F7\u06F8\u06F9]/g, function (e) { return String.fromCharCode(e.charCodeAt(0) - 1728) }).replace(/[\u0660\u0661\u0662\u0663\u0664\u0665\u0666\u0667\u0668\u0669]/g, function (e) { return String.fromCharCode(e.charCodeAt(0) - 1584) }).replace(/[\u066B]/g, "."); return origParseInt(e) }; origParseFloat = parseFloat; parseFloat = function (e) { e = e && e.toString().replace(/[\u06F0\u06F1\u06F2\u06F3\u06F4\u06F5\u06F6\u06F7\u06F8\u06F9]/g, function (e) { return String.fromCharCode(e.charCodeAt(0) - 1728) }).replace(/[\u0660\u0661\u0662\u0663\u0664\u0665\u0666\u0667\u0668\u0669]/g, function (e) { return String.fromCharCode(e.charCodeAt(0) - 1584) }).replace(/[\u066B]/g, "."); return origParseFloat(e) }