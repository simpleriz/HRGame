https://www.figma.com/design/UlkfXBYoaISQq4TXYvewOg/Untitled?node-id=3-4&m=dev&t=nBune5CABUi0J1AT-1

https://lucid.app/lucidspark/71ca598e-02d6-47f9-83ee-c6d15e3d4f3b/edit?invitationId=inv_c60a759a-43c1-40e1-a1df-504c4dd00466
Желтые прямоугольники - кнопки
Зеленые овалы - подсказки (в подсказке написан итоговый текст)
Красные круги - окна
Голубые ромбы - итог
#### Окна
**Общие правила:**
У окон есть крестик и минус (как у обычных окон в windows). Крестик закрывает окно так, что открыть его можно только заново нажав на hotkey или кнопку на экране. Минус передвигает окно на задний план относительно всех других открытых окон. Каждое новое окно автоматически открывается на переднем плане. При нажатии на окно находящееся на на переднем плане оно перемещается на передний. При зажатии на title bar сверху окна вне крестика и минуса игрок передвигает окно в произвольном режиме. При наведении на крестик - цвет заливки становится красным, при наведении на минус - темно серым. Изменять размер окон невозможно.

**Текст:**
Текст всегда ровняется по левому краю, если не указано иное. Текст в кнопках ровняется по центру. Шрифт на данный момент не важен, потом изменится. 

**Окна в игре:**
- Резюме
- Телефон
- Телефонный справочник
- Персонажи
- Новостная лента
- Спец Действия
**Окно вербовки нового персонажа**
Окно вербовки нового персонажа отличается от остальных. Его нельзя свернуть и оно всегда находится на самом переднем плане. Его нельзя передвинуть. Расположение снизу справа, над кнопками действий.
**Окно подтверждения действия**
Окно подтверждения действия вызывается в отдельных случаях, например при увольнении персонажа. Его нельзя свернуть и оно всегда находится на самом переднем плане. Его нельзя передвинуть. Расположение по центру.

**Изначальное место появления окон:**
5% от экрана с каждой стороны являются мертвой зоной. Например при появлении окна слева, оно появляется с отступом в 5% от размера игры. Снизу и сверху 5% отступ идет не от границы игры, а от верхней границы внутриигровых кнопок (снизу) и часов (сверху). 
- Резюме - Вертикаль: Центр .Горизонталь: Справа. При появлении нескольких резюме каждое последующее появляется с отступом в 5% от размера игры влево и вниз. При достижении нижней границы перестает смещаться вниз.
- Телефон - Вертикаль: Снизу .Горизонталь: Справа.
- Телефонный справочник - Вертикаль: Сверху .Горизонталь: Справа.
- Персонажи - Вертикаль: Центр .Горизонталь: Слева.
- Новостная лента - Вертикаль: Снизу .Горизонталь: Справа.
- Спец Действия - Вертикаль: Снизу. Горизонталь: Центр.
- Окно вербовки нового персонажа - Вертикаль: Центр .Горизонталь: Центр .
- Окно подтверждения действия - Вертикаль: Центр .Горизонталь: Центр .
#### Внутриигровые кнопки
Всего в игре 5 кнопок. Каждая из них вызывает соответствующие окно. 
В дальнейшем на каждой из кнопок будет изображение, пока его нет нужно раскрасить кнопки как в FIGMA и добавить 1-2 буквы для обозначения.
Hotkeys:
Открыть окно персонажей - С
Открыть окно телефона - T
Открыть телефонную книгу - B
Открыть новостную ленту - N
Открыть окно специальных действий - A
Закрыть все открытые окна - shift + Q

#### Подсказки
При наведении на кнопки и некоторые надписи в окнах всплывают подсказки.
Подсказка всплывает через 1 секунду после наведения на тригер и исчезает через 0.5 секунды после того, как игрок уберет курсор.

#### Главное меню
#### Выбор сложности
#### Меню настроек