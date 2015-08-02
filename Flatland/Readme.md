Flatland
=========
Il programma si trova in *Program.fs*     

Sono state definite tre classi per i bottoni e due per il programma principale:     
- **btnScroll**: per i bottoni di scroll
- **btnToggle**: per i bottoni on/off
- **btnPlusMinus**: per i bottoni di incremento e diminuzione
- **Planet**: modella un pianeta
- **Flatlandia**: implementa l’universo simulato

Tutte le funzioni sono accessibili tramite l’interfaccia. Inoltre è possibile aggiungere un pianeta con un
*click&drag* e impostare un immagine con un doppio click sul pianeta.

Il colore dei pianeti viene scelto in base alla loro massa, considerandoli come stelle con una temperatura
proporzionale alla massa.

Nel caso di collisione di due pianeti il pianeta più piccolo viene inglobato dal più grande, e viene calcolato un
urto elastico totale.

Per le immagini del pianeta non è stata adottata nessuna funzione di crop, dato che è presumibile vengano
utilizzate delle immagini di pianeti con lo sfondo trasparente (vengono fornite alcune immagini di esempio
nell’archivio).

Per l’animazione lo spostamento viene calcolato in tanti piccoli step per cercare di ridurre l’errore il più
possibile. Nel caso di un unico step infatti quando due pianeti sono troppo vicini l’accelerazione è troppo alta e
potrebbe capitare che i pianeti si “scavalchino”, senza entrare in collisione e acquisendo una velocità tale da
sparire dallo schermo. Eistono anche altri metodi di calcolo, come i metodi di Runge-Kutta o l’integrazione di
Verlet.

Vengono usati due timer, uno per l’animazione e uno per rendere coerente il comportamento dell’interfaccia con
quello della tastiera (simulazione della ripetizione dei tasti se tenuti premuti)
Sono presenti anche le shortcut da tastiera (testate sul notebook di sviluppo):
- **W** : Scrolla in alto
- **S** : Scrolla in basso
- **A** : Scrolla a sinistra
- **D** : Scrolla a destra
- **Z** : ZoomOut
- **X** : ZoomIn
- **M**: Aumenta densità griglia del campo di forze
- **N**: Diminuisci densità griglia del campo di forze
- **F1**: Attiva/Disattiva visualizzazione direzione del campo di forze
- **F2** : Attiva/Disattiva visualizzazione intensità del campo di forze
- **F3**: Attiva/Disattiva visualizzazione delle accelerazioni
- **ESC**: Chiudi programma
- **C**: Resetta la simulazione

Problemi noti:
· Il caricamento di un immagine in uno stato avanzato della simulazione rallenta in modo anomalo la
simulazione. Non è stato possibile trovarne la causa per via del poco tempo a disposizione







