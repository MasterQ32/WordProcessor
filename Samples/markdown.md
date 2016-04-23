

# Dies ist eine Überschrift.

Dies ist der erste Absatz des Textes. Er ist
angenehm lang und hat ein paar Zeilen Text.

Dies ist der zweite Absatz. Er beginnt mit
einem sehr ähnlichen Satz zu dem erste Absatz,
hat aber einen anderen Inhalt.

## Floating Elements
Not supported in Markdown v1.0

## Textformatierung

Hallo, dies ist ein **fetter** Text.

Hallo, dies ist ein *schräger* Text.

Hallo, die ist ein [Link](http://google.de).

## Bildelemente
![](file://C:/dump/0be2d4f4ad3ea1402bdca975edfc793a0e0767e9.jpeg)

# Überschrift 1

Dies ist ein einleitender Text
welcher noch keinen Zeilenumbruch
besitzt.

Im zweiten Absatz aber
entsteht ein Abstand zum ersten Absatz.

[Link zu Google](http://google.de)

![Profilbild](file://C:/dump/7d57a81fa8a45ac3e545ff9b26237255fb7d33fd.jpeg)

$$ f(x) = a * x^2 + b*x * c $$ 

:grin:

## Unterkapitel 1

Dies ist das erste Unterkapitel, welches
eine aufzählende Liste enthält:

1. Erstes Element
2. Zweites Element
	1. Erster Unterpunkt
	2. Zweiter Unterpunkt
3. Drittes Element

## Unterkapitel 2

Dieses Unterkapitel enthält eine
unsortierte Liste:

- Erstes Element
	- Unterelement
		- Weiteres Unterelement
	- Unterelement
- Zweites Element
	- Erstes Unterelement
	- Zweites Unterelement
		- Unterunterelement
		- Superunterelement
- Drittes Element

## Unterkapitel 3

Eine Tabelle sieht folgendermaßen aus:

| Spalte 1 | Spalte 2 |
|----------|----------|
| a        | b        |
| c        | d        |

Eine Tabelle kann aber auch gekürzt
geschrieben werden:

Spalte 1|Spalte 2
-|-
a|b
c|d

### Unterunterkapitel 1

Markdown hat Support für Code-Blöcke:

	int fac(int i) {
		if(i <= 1)
			return 1;
		else
			return fac(i - i) * i;
	}

Es gibt auch Support für `inline` Code. **Fetter** Text wird mit zwei `**` geschrieben, *kursiver* Text mit nur einem `*`.