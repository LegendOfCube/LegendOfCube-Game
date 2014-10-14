# Legend of Cube

## Bakgrund
Plattformspel har inte fått lika mycket fokus under 3D-eran som de hade under 2D-eran. En trolig anledning till detta är att det är betydligt svårare att ge spelaren samma nivå av kontroll i 3D som de hade i 2D. Du har fler riktningar du kan röra dig i och mer potential för misstag samtidigt som du har en kamera som kanske inte alltid visar exakt det som spelaren behöver se. Det har trots detta gjorts många bra försök, framförallt spel såsom Super Mario 64 och Super Mario Galaxy från Nintendo. Många skulle dock argumentera att man i dessa spel fortfarande inte har samma nivå av kontroll som man har i moderna 2D spel såsom Super Meat Boy. 

## Projektbeskrivning
Ett 3D plattformsspel som utspelar sig i en labyrint. Labyrinten består av ett antal sammankopplade rum där varje rum innehåller någon form av plattformshinder man behöver ta sig förbi. Det finns ett antal olika uppgraderingar gömda i labyrinten, en sådan uppgradering kan vara förmågan att göra vägghopp. En del rum i labyrinten kan enbart passeras om man har specifika uppgraderingar, så för att ta sig igenom hela labyrinten måste man leta efter uppgraderingar som tillåter än att ta sig igenom tidigare opasserbara rum.

Mycket fokus kommer ligga på att få spelaren att känna att hon har full kontroll över karaktären och vad som händer i spelet. Att implementera en hoppmekanik är inte så enkelt som det låter, spelaren måste känna att hon hela tiden har full kontroll över hoppet. Det innebär att man behöver kunna avbryta ett påbörjat hopp och även ändra riktning i luften. Utöver detta så ska hoppet dessutom kännas och se bra ut. För att åstadkomma detta och övriga plattformselement ska en egen fysikmotor implementeras. Fysikmotorn kommer inte att följa riktiga fysik eller vara överdrivet komplicerad, målet är att göra något som ger spelaren full kontroll och som känns bra. En annan sak som krävs för att ge spelaren kontroll är en bra kamera. Kameran får inte hamna ivägen och måste se till att spelaren kan bedöma avstånd och vinklar korrekt.

Utöver den tekniska aspekten av spelet så kommer huvuddelen av fokus ligga på att designa intressanta plattformshinder och en bra labyrint. Ett bra plattformshinder ska vara en utmaning men inte orättvist, spelaren ska inte överraskas av saker hon inte har kontroll över. Dessutom så bör ett bra plattformshinder kunna överkommas snabbt och graciöst om man vet hur man ska göra, ett hinder där man tvingas vänta mycket på att saker ska flytta på sig är troligen inte särskilt bra. En bra labyrint skulle kunna ha olika möjliga vägar till målet, det ger även mer "replay value". Eventuellt skulle man kunna slumpa själva labyrinten och bara designa rummen för hand.

Utvecklingen kommer att ske i C++ med SDL + Ogre3D eller i Unreal Engine 4. Eftersom huvudkaraktären är en kub så bör det inte vara några problem att klara av projektet om ingen medlem har erfarenhet av att göra modeller eller texturer.

## Prerequisites
- Grundläggande erfarenhet av C++

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
- Inget krav, men väldigt bra om erfarenhet av att göra 3D-modeller och texturer.

### Handledare
???

### Institution
Data- och informationsteknik
