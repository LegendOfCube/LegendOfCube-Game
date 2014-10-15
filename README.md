# Legend of Cube

## Bakgrund
Sedan övergången till 3D har plattformsspel inte fått lika mycket fokus som de hade under 2D-eran. En trolig anledning till detta är att det är betydligt svårare att ge spelaren samma nivå av kontroll i 3D som de hade i 2D. Du har fler riktningar du kan röra dig i och därmed mer potential för misstag. Detta förstärks av att du har en kamera som kanske inte alltid visar exakt det som spelaren behöver se. Några lyckade exempel på plattformsspel i 3D är Super Mario 64 och Super Mario Galaxy från Nintendo. Många skulle dock argumentera att man i dessa spel fortfarande inte har samma nivå av kontroll som man har i moderna 2D spel såsom Super Meat Boy. 

## Projektbeskrivning
Legend of Cube är ett 3D plattformsspel som utspelar sig i en labyrint med en kub som huvudkaraktär. Labyrinten består av ett antal sammankopplade rum där varje rum innehåller någon form av plattformshinder man behöver ta sig förbi. Det finns ett antal olika uppgraderingar gömda i labyrinten, en sådan uppgradering kan t.ex. vara förmågan att göra vägghopp. En del rum i labyrinten kan enbart passeras om man har specifika uppgraderingar, så för att ta sig igenom hela labyrinten måste man leta efter uppgraderingar som tillåter än att ta sig igenom tidigare icke-passerbara rum.

Mycket fokus kommer ligga på att ge spelaren full kontroll över karaktären och dess rörelser. Att implementera hoppmekaniken är inte helt trivialt då spelaren behöver mer kontroll än riktig fysik tillåter. T.ex. så vill man troligen kunna avbryta ett påbörjat hopp eller byta riktning i luften. För att åstadkomma detta och andra plattformselement så ska en egen fysikmotor utvecklas. Fysikmotorn kommer inte vara överdrivet komplicerad och kommer inte heller följa riktig fysik. Målet är att ge bästa möjliga kontroll till spelaren och möjliggöra intressanta plattformshinder. Ett komplement till detta är givetvis att se till att kameran inte hindrar spelaren från att bedöma avstånd och vinklar korrekt. Den bästa kameran är den man inte tänker på.

Ett vanligt problem i spel är att göra bra modeller och texturer, men eftersom huvudkaraktären är en kub så behöver inte särskilt avancerade saker göras. Men om intresse i gruppen finns kan tid givetvis läggas på detta. Antagligen kommer mer tid läggas på att skriva bra shaders och liknande. Överlag är det viktigaste att grafiken är ren och tydlig, grafiken får inte bli ett hinder för spelaren.

Utöver den tekniska aspekten av spelet så kommer huvuddelen av fokus ligga på att designa intressanta plattformshinder och en bra labyrint. Ett bra plattformshinder ska vara en utmaning men inte orättvist, spelaren ska inte överraskas av saker hon inte har kontroll över. Dessutom så bör ett bra hinder kunna överkommas snabbt och graciöst av en erfaren spelare. En bra labyrint skulle kunna ha olika möjliga vägar till målet, det ger även mer omspelningsvärde. Eventuellt skulle man kunna slumpa själva labyrinten och bara designa rummen för hand.

Utvecklingen kommer att ske i C++ med SDL + Ogre3D eller i Unreal Engine 4.

### Förslagslämnare
- Peter Hillerström
- Lucas Persson

### Projektkod
???

### Målgrupp
D och IT

### Gruppstorlek
4-6 personer

### Speciella förkunskapskrav
- Objektorienterad programmering
- Grundläggande erfarenhet av C++
- Inget krav, men väldigt bra med erfarenhet av att göra 3D-modeller och texturer.

### Handledare
???

### Institution
Data- och informationsteknik
