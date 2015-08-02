// Gian Maria Delogu     452706

open System.Windows.Forms
open System.Drawing
open System.Drawing.Drawing2D
//
/////////////////// BUTTONS ///////////////
//pens and brushes for button
let btnFatPen = new Pen(Color.Snow, 2.f)
let btnPen = btnFatPen //Pens.Snow
let btnBrush = new SolidBrush(Color.Snow) 
let btnAlphaBrush = new SolidBrush(Color.FromArgb(150, Color.Snow))
let brushA = new SolidBrush(Color.FromArgb(200, Color.LimeGreen))
let brushD = new SolidBrush(Color.FromArgb(200, Color.Red))
let btnPenSelected = new Pen(Color.Lime, 2.f)

type btnScroll() =
    let mutable rect = Rectangle()
    let mutable dir = ""
    let mutable selected = false    
    let mutable actualpen = btnPen
    
    member x.Action
        with get () = dir
        and set(v) = dir <- v

    member x.ClientRectangle
        with get() = rect
        and set (r) = rect <- r
    
    member x.Select() =
            actualpen <- btnPenSelected
            selected <- true
    
    member x.Unselect() =
            actualpen <- btnPen
            selected <- false

    member x.Contains(p:Point) = rect.Contains(p)

    member x.Paint(g:Graphics) =
        let l, t = rect.Left, rect.Top
        let w, h = rect.Width, rect.Height
        let backup = g.Save()
        g.TranslateTransform(single l, single t)
        
         
        match dir with
            | "UP" -> 
                g.FillPolygon(btnAlphaBrush, [| Point(w/2, 0); Point(w, h); Point(0, h) |])
                g.DrawPolygon(actualpen, [| Point(w/2, 0); Point(w, h); Point(0, h) |])

            | "DOWN" ->
                g.FillPolygon(btnAlphaBrush, [|Point(); Point(w, 0); Point(w/2, h) |]) 
                g.DrawPolygon(actualpen, [|Point(); Point(w, 0); Point(w/2, h) |])
            | "LEFT" ->
                g.FillPolygon(btnAlphaBrush, [| Point(0, h/2); Point(w, 0); Point(w, h) |])
                g.DrawPolygon(actualpen, [| Point(0, h/2); Point(w, 0); Point(w, h) |])
            | "RIGHT" ->
                g.FillPolygon(btnAlphaBrush, [| Point(); Point(w, h/2); Point(0, h) |])
                g.DrawPolygon(actualpen, [| Point(); Point(w, h/2); Point(0, h) |])
            | _ -> ()
            
        g.Restore(backup)
        
           
type btnToggle() =
    let mutable rect = Rectangle()
    let mutable la = ""
    let mutable ld = ""
    let mutable action = ""
    let font = new Font(FontFamily.GenericSansSerif, 10.0F, FontStyle.Italic)

    let mutable active = false

    member x.Action
        with get () = action
        and set(v) = action <- v

    member x.ClientRectangle
        with get() = rect
        and set (r) = rect <- r

    member x.labelActive
        with get () = la
        and set (v) = la <- v

    member x.labelDisabled
        with get() = ld
        and set(v) = ld <- v

    member x.Status 
        with get() = active
        and set(v) = active <- v

    member x.Contains(p:Point) = rect.Contains(p)

    member x.Toggle() = 
        if active then active <- false else active <- true

    member x.Paint(g:Graphics) =
        let l, t = rect.Left, rect.Top
        let w, h = rect.Width, rect.Height
        let backup = g.Save()
      
        let actualbrush =
            if action = "NONE" then btnAlphaBrush //for the inactive zoom button
            elif active then brushA else brushD
        
        g.FillRectangle(actualbrush, rect)
        g.DrawRectangle(btnPen, rect)
        g.TranslateTransform(single l, single t)
        let sz = g.MeasureString(la, font)
        let left = (single(w) - sz.Width) /2.f 
        let top = (single(h) - sz.Height) /2.f
        let sz2 = g.MeasureString(ld, font)
        let left2 = (single(w) - sz2.Width) /2.f  
        let top2 = (single(h) - sz.Height)  /2.f              

        if active then
            //g.DrawString(la, font, Brushes.Snow, left + 1.f, top + 1.f) //shadow effect
            g.DrawString(la, font, btnBrush, left, top)
        else
            //g.DrawString(ld, font, Brushes.Snow, left2 + 1.f, top2 + 1.f) //shadow effect
            g.DrawString(ld, font, btnBrush, left2, top2)
        
        g.Restore(backup)
       


type btnPlusMinus() =
    let mutable rect = Rectangle()
    let mutable action = ""
    let mutable sign = ""
    let mutable selected = false    
    let mutable actualpen = btnPen

    member x.Action
        with get () = action
        and set(v) = action <- v

    member x.ClientRectangle
        with get() = rect
        and set (r) = rect <- r

    member x.Sign
        with get() = sign
        and set(v) = sign <- v
     
    member x.Select() =
            actualpen <- btnPenSelected
            selected <- true
    
    member x.Unselect() =
            actualpen <- btnPen
            selected <- false

    member x.Contains(p:Point) = rect.Contains(p)

    member x.Paint(g:Graphics) =
        let l, t = rect.Left, rect.Top
        let w, h = rect.Width, rect.Height
        let backup = g.Save()
  
        g.FillEllipse(btnAlphaBrush, rect)
        g.DrawEllipse(actualpen, rect)

        g.TranslateTransform(single l, single t)
        if sign = "+" then g.DrawLine(actualpen, w/2, 3, w/2, h-3) 
        g.DrawLine(actualpen, 3, h/2, w-3, h/2)                  
        g.Restore(backup)
    


///////////////// PLANET AND SPACE ///////////////////
// http://www.vendian.org/mncharity/dir3/blackbody/UnstableURLs/bbr_color.html
let kelvinColors = [|
    new SolidBrush(Color.FromArgb(255, 255, 51, 0));
    new SolidBrush(Color.FromArgb(255, 255, 130, 0));
    new SolidBrush(Color.FromArgb(255, 255, 165, 79));
    new SolidBrush(Color.FromArgb(255, 255, 196, 137));
    new SolidBrush(Color.FromArgb(255, 255, 253, 215));
    new SolidBrush(Color.FromArgb(255, 255, 242, 236));
    new SolidBrush(Color.FromArgb(255, 236, 238, 255));
    new SolidBrush(Color.FromArgb(255, 216, 224, 255));
    new SolidBrush(Color.FromArgb(255, 216, 224, 255));
    new SolidBrush(Color.FromArgb(255, 203, 215, 255));
    new SolidBrush(Color.FromArgb(255, 183, 206, 255));
    new SolidBrush(Color.FromArgb(255, 166, 195, 255));
    |]

type Planet(center: PointF) =
    
    let mutable myrect = new Rectangle()
    let mutable mycenter = new PointF()
    let mutable mymass = 0.f
    let mutable myradius = 0
    let mutable myaccx = 0.f
    let mutable myaccy = 0.f
    let mutable myvelx = 0.f
    let mutable myvely = 0.f

    let mutable img : Bitmap  = null
    let mutable tBrush : TextureBrush = null
    

    let mutable collided = false

    let colorcutoff = 500000.f // per la variazione del colore in funzione della massa

    do
        mycenter <- center
        mymass <- 0.f
        myradius <- 0
    
    /////////// Public Variables  ////////////
    member x.Center
        with set(v) = mycenter <- v
        and get() = mycenter

    member x.Mass
        with get() = mymass

    member x.Rect 
        with set(v) = myrect <- v
        and get() = myrect 

    member x.Radius with get() = myradius
    
    member x.ColorCutOff with get() = colorcutoff

    member x.Velx with get() = myvelx
    member x.Vely with get() = myvely

    member x.Img with set(v) = img <- v

    
    ///////// Public methods  ///////////////
    
    member x.Collided() =
        collided <- true
        mymass <- 0.f

    member x.Inglobe(planet:Planet) =
        
        //urto anaelastico totale
        myvelx <- (float32 mymass * myvelx + float32 planet.Mass * planet.Velx )/float32 (mymass + planet.Mass)
        myvely <- (float32 mymass * myvely + float32 planet.Mass * planet.Vely )/float32 (mymass + planet.Mass)

        
        mymass <- mymass + planet.Mass
        myradius <- int(((float32 mymass)/4.f) ** (1.f/3.f))

        planet.Collided()

    member x.NewRadius(m : float32) =
        myradius <- int m
        myrect <- new Rectangle(int mycenter.X - myradius, int mycenter.Y - myradius, myradius * 2, myradius * 2)
        mymass <- float32 (4 * myradius * myradius * myradius)
    
    member x.Move(elapsed, w) =
        myvelx <- myvelx + w * (myaccx * elapsed)
        myvely <- myvely + w * (myaccy * elapsed)
        let movx = w * (myvelx * elapsed) //+ (0.5f * myaccx * float32 elapsed**2.f) 
        let movy = w * (myvely * elapsed) //+ (0.5f * myaccy * float32 elapsed**2.f)
        x.Center <- new PointF(x.Center.X + movx , x.Center.Y + movy)
        myrect <- new Rectangle(int mycenter.X - myradius, int mycenter.Y - myradius, myradius * 2, myradius * 2)

    member x.PlanetAcc(planets:ResizeArray<Planet>) =
        
        let mutable ax = 0.f
        let mutable ay = 0.f
        
        let mutable collision = false


        for pp = 0 to planets.Count - 1 do
                
            if x <> planets.[pp] then  
                let mutable partialforce = 0.f
                let distancex = planets.[pp].Center.X - x.Center.X
                let distancey = planets.[pp].Center.Y - x.Center.Y
                let distance = sqrt(float32(distancex)**2.f + float32(distancey)**2.f)
                        
                if distance <> 0.f then 
                    partialforce <- float32(x.Mass) * float32(planets.[pp].Mass)/ float32(distance)**2.f
                    
                    let sinb = distancey / distance
                    let cosb = distancex / distance
                    
                    ax <- ax + float32(partialforce) * cosb / float32(x.Mass)
                    ay <- ay + float32(partialforce) * sinb / float32(x.Mass)
        

        myaccx <- ax / 100.f
        myaccy <- ay /100.f  //divido per 100 perchè altrimenti troppo veloce
        
    
    member x.LoadImage(path:string) =
        if img <> null then img.Dispose()
        img <- new Bitmap(myrect.Width, myrect.Height)
        use g = Graphics.FromImage(img)
        g.DrawImage(Image.FromFile(path), 0, 0, myrect.Width, myrect.Height)
      
    
    member x.Paint(g:Graphics, showAcc) =
        
        g.FillEllipse( kelvinColors.[min (int(x.Mass / colorcutoff)) (kelvinColors.Length-1)], myrect)
        g.DrawEllipse( Pens.GhostWhite, myrect)
        if img <> null then
            g.DrawImage(img, myrect)
            
        //accelerazione, moltiplico per 100 per renderla più visibile
        if (myaccx <> 0.f || myaccy <> 0.f) && showAcc then g.DrawLine(Pens.Red, x.Center.X, x.Center.Y, x.Center.X + (myaccx*100.f) , x.Center.Y + (myaccy*100.f) )



type Flatlandia() as x =
    inherit UserControl()

    let mutable planets = new ResizeArray<Planet>()
    let mutable newPlanet = new Planet(PointF(0.f,0.f))

    let mutable clicked = false
    let mutable animated = false
    //let mutable lastRefresh = int64 0
    //let mutable timer = System.Diagnostics.Stopwatch()
    let mutable clickpx = 0
    let mutable clickpy = 0
    let mutable planetclick = false
    let mutable doubleclick = false
    let mutable planetclicked = newPlanet

    let ti = 33
    let timertick = new Timer(Interval=ti) //animation timer

    let mutable scrollx = 0
    let mutable scrolly = 0
    let mutable zoom = 1.f

    let mutable squaresInterval = 25
    let mutable squaresDimension = 4
    let mutable scrollSpeed = 15

    let mutable showLines = true
    let mutable showSquares = true
    let mutable showAcc = false

    let mutable interfacew = 200
    let mutable interfaceh = 200
    let mutable interfacemargin = 4
    

    let mutable buttonclick = false
    let mutable action = "NONE"
    let continueAction = new Timer(Interval=50) //continue interface action timer
    
    let mutable scrollButtons = [|
        btnScroll(ClientRectangle = Rectangle((interfacew/2) - 20, 0, 40, 20), Action = "UP");
        btnScroll(ClientRectangle = Rectangle(interfacew/2 - 20, interfaceh - 20, 40, 20) , Action =  "DOWN"); 
        btnScroll(ClientRectangle = Rectangle(0, interfaceh/2 - 20, 20, 40), Action = "LEFT");      
        btnScroll(ClientRectangle = Rectangle(interfacew - 20, interfaceh/2 - 20, 20, 40), Action = "RIGHT");
        |]
    let mutable toggleButtons = [|
        btnToggle(ClientRectangle = Rectangle((interfacew/2) - 40, 30, 80, 20), Action = "StartStop", labelActive="Stop ■", labelDisabled = "Play ►", Status = animated);
        btnToggle(ClientRectangle = Rectangle((interfacew/2) - 40, 90, 80, 20), Action = "ToggleAcceleration", labelActive="Acceleration", labelDisabled="Acceleration", Status = showAcc);
        btnToggle(ClientRectangle = Rectangle((interfacew/2) - 40, 120, 80, 20), Action = "ToggleSquares", labelActive="Intensity", labelDisabled="Intensity", Status = showSquares);
        btnToggle(ClientRectangle = Rectangle((interfacew/2) - 40, 150, 80, 20), Action = "ToggleLines", labelActive="Direction", labelDisabled="Direction", Status = showLines);
        btnToggle(ClientRectangle = Rectangle((interfacew/2) - 40, 60, 80, 20), Action = "NONE", labelActive="Zoom", labelDisabled = "Zoom");
          
        |]
    let mutable pmButtons = [|
        btnPlusMinus(ClientRectangle = Rectangle((interfacew/2) - 70, 60, 20, 20), Action = "ZoomIn", Sign = "-");
        btnPlusMinus(ClientRectangle = Rectangle((interfacew/2) + 50, 60, 20, 20), Action = "ZoomOut", Sign = "+");
        btnPlusMinus(ClientRectangle = Rectangle((interfacew/2) - 70, 132, 20, 20), Action = "FieldInterval-", Sign = "-");
        btnPlusMinus(ClientRectangle = Rectangle((interfacew/2) + 50, 132, 20, 20), Action = "FieldInterval+", Sign = "+");
        |]
    

    
    do
        x.SetStyle(ControlStyles.OptimizedDoubleBuffer, true)
        x.SetStyle(ControlStyles.AllPaintingInWmPaint, true)

        timertick.Tick.Add(fun _ ->
            x.eppurSiMuove()
        )

        continueAction.Tick.Add(fun _ ->
            match action with
            | "UP" -> x.Scroll("U")
            | "DOWN" -> x.Scroll("D")
            | "LEFT" -> x.Scroll("L")
            | "RIGHT" -> x.Scroll("R") 
            | "ZoomOut" -> x.Zoom("IN")
            | "ZoomIn" -> x.Zoom("OUT")
            | "FieldInterval-" -> x.modsquaresInterval("-")
            | "FieldInterval+" -> x.modsquaresInterval("+")
            | _ -> ()
        )
        
        
       
    
    ///////// Public variables  ///////////////

    member x.isMoving with get() = animated    
    
    ///////// Public methods  ///////////////

    member x.Scroll(direction) =
        match direction with
        | "U" -> scrolly <- scrolly + int(float32 scrollSpeed)// / zoom)
        | "D" -> scrolly <- scrolly - int(float32 scrollSpeed)// / zoom)
        | "L" -> scrollx <- scrollx + int(float32 scrollSpeed)// / zoom)
        | "R" -> scrollx <- scrollx - int(float32 scrollSpeed)// / zoom)
        | _ -> ()
        //x.PrintInfoPos()

        x.Invalidate()

    member x.StartStop() =
        if animated then 
            //timer.Stop()
            timertick.Stop()
            animated <- false 
        else 
            //timer.Start()
            timertick.Start()
            animated <- true
        //x.PrintInfoPos()
        //per tenere aggioranto lo stato del pulsante:
        toggleButtons.[0].Toggle()
        x.Invalidate(Rectangle(x.Width - interfacew - interfacemargin, interfacemargin, interfacew, interfaceh))
       
    
    member x.Zoom(direction) =
        match direction with
        | "IN" -> zoom <- zoom * 1.1f
        | "OUT" -> zoom <- zoom / 1.1f
        | _ -> ()
        //x.PrintInfoPos()
        x.Invalidate()

    member x.ToggleLines() =
        if showLines then showLines <- false
        else showLines <- true
        //per tenere aggioranto lo stato del pulsante:
        toggleButtons.[3].Toggle()
        x.Invalidate()
    

    member x.ToggleSquares() =
        if showSquares then showSquares <- false
        else showSquares <- true
        //per tenere aggioranto lo stato del pulsante:
        toggleButtons.[2].Toggle()
        x.Invalidate()

    member x.modsquaresInterval(operator) =
        match operator with
        | "+" -> if squaresInterval > 6 then squaresInterval <- squaresInterval - 1
        | "-" -> squaresInterval <- squaresInterval + 1
        | _ -> ()
        
        x.Invalidate()

    member x.ToggleAcc() =
        if showAcc then showAcc <- false else showAcc <- true
        //per tenere aggioranto lo stato del pulsante:
        toggleButtons.[1].Toggle()
        x.Invalidate()
    
    //reset univers
    member x.BigCrush() = 
        planets <- new ResizeArray<Planet>()
        zoom <- 1.f
        scrollx <- 0
        scrolly <- 0
        x.PrintInfoPos()
        if animated then x.StartStop()
        x.Invalidate()
    
    member x.eppurSiMuove() =
        
        //let now = timer.ElapsedMilliseconds
        //let mutable elapsed = now - lastRefresh
        
        //improved eulero's method
        let it = 128
        let w = 1.f / float32 it
        for i = 0 to it do
            if planets.Count > 0 then
                x.Collision()               
                for p = 0 to planets.Count - 1 do
                    planets.[p].PlanetAcc(planets)
                for p = 0 to planets.Count - 1 do
                    planets.[p].Move(float32 ti, w)
                x.Invalidate()
        

        //lastRefresh <- now
    

    member x.PrintInfoPos() =
        printfn "Scrollx: %d | Scrolly: %d | Zoom %f | Animated %b | Planets: %d" scrollx scrolly zoom animated planets.Count 

       
           
    member private x.Collision() =
        let mutable collided = new ResizeArray<Planet>() 

        for p = 0 to planets.Count - 1 do
            for pp = 0 to planets.Count - 1 do
                if p <> pp && planets.[p].Mass > 0.f && planets.[pp].Mass > 0.f then
                    let mutable partialforce = 0.f
                    let distancex = planets.[p].Center.X - planets.[pp].Center.X
                    let distancey = planets.[p].Center.Y - planets.[pp].Center.Y
                    let distance = sqrt(float32(distancex)**2.f + float32(distancey)**2.f)

                    if distance <= float32(planets.[p].Radius) + float32 (planets.[pp].Radius) then 
                        if planets.[p].Mass > planets.[pp].Mass then
                            collided.Add(planets.[pp])
                            planets.[p].Inglobe(planets.[pp])
                        else
                            collided.Add(planets.[p])
                            planets.[pp].Inglobe(planets.[p])
                                  
        
        for ppp = 0 to collided.Count - 1 do
            planets.Remove(collided.[ppp]) |> ignore


    member private x.ForcesField(g:Graphics) =
        let mutable alternateLines = 0
        let startpointx = int (float32 x.Width/(2.f * zoom)) + int(float32 scrollx / zoom)
        let startpointy = int (float32 x.Height/(2.f * zoom)) + int(float32 scrolly / zoom)
        let endpointx = int (float32 x.Width/zoom) -  int(float32 scrollx / zoom)
        let endpointy = int (float32 x.Height/zoom) - int(float32 scrolly / zoom)
        let scaledSquareInterval = int(float32 squaresInterval / zoom)
        let scaledSquareDimension = int(float32 squaresDimension / zoom)
        for xx in -startpointx .. scaledSquareInterval .. endpointx - 1 do
            for yy in -startpointy .. scaledSquareInterval .. endpointy - 1 do
                
                let mutable fx = 0.f
                let mutable fy = 0.f
                let mutable totalforce = 0
                let mutable insideaplanet = false

                for p = 0 to planets.Count - 1 do
                    if not insideaplanet then 
                        let mutable partialforce = 0.f
                        let distancex = planets.[p].Center.X - float32 xx
                        let distancey = planets.[p].Center.Y - float32 yy
                        let distance = sqrt(float32(distancex)**2.f + float32(distancey)**2.f)
                        
                        if distance <= float32(planets.[p].Radius) then insideaplanet <- true
                        if distance <> 0.f && not insideaplanet then 
                            partialforce <- float32(planets.[p].Mass) / float32(distance)**2.f
                    
                            let sinb = float32(distancey) / distance
                            let cosb = float32(distancex) / distance
                    
                            fx <- fx + float32(partialforce) * cosb
                            fy <- fy + float32(partialforce) * sinb

                if not insideaplanet then 
                    totalforce <- int(sqrt(fx**2.f + fy**2.f))
                    let forcecolor = min totalforce 255 
                    use brush = new SolidBrush( Color.FromArgb(255, forcecolor, forcecolor, forcecolor))
                    use alphabrush = new SolidBrush( Color.FromArgb(150, forcecolor, forcecolor, forcecolor))
                    
                    if showLines then
                        use pen = new Pen(alphabrush) 
                        if totalforce > 100 then
                            g.DrawLine(pen, float32(xx+1), float32(yy+1), float32(xx) - (fx/10.f) , float32(yy) - (fy/10.f) )
                    
                    if showSquares then g.FillRectangle(brush, Rectangle(xx, yy, scaledSquareDimension, scaledSquareDimension))
 
    
    override x.OnPaint e =
        base.OnPaint(e)

        let g = e.Graphics
        g.SmoothingMode <- Drawing2D.SmoothingMode.AntiAlias        
        g.TranslateTransform(float32 scrollx, float32 scrolly)  
        g.ScaleTransform(zoom, zoom)  

        //draw forces field
        if planets.Count > 0 && (showLines || showSquares) then x.ForcesField(g)

        //draw planets
        if planets.Count > 0 then
            
            for i=0 to planets.Count - 1 do
                if showAcc then planets.[i].PlanetAcc(planets)
                planets.[i].Paint(g, showAcc)
         
        //draw new planet
        if newPlanet.Mass > 0.f then
            g.FillEllipse( new SolidBrush(Color.FromArgb(150, kelvinColors.[min (int(newPlanet.Mass / newPlanet.ColorCutOff)) (kelvinColors.Length-1)].Color )), newPlanet.Rect)
            g.DrawEllipse( Pens.NavajoWhite, newPlanet.Rect)
        
        g.ResetTransform()

        //draw interface
        
        g.TranslateTransform(float32 (x.Width - interfacew - interfacemargin), float32 interfacemargin)

        g.DrawEllipse( btnFatPen, -interfacemargin, -interfacemargin, interfacew + 2 * interfacemargin, interfaceh + 2* interfacemargin)
        g.FillEllipse(new SolidBrush(Color.FromArgb(150, 61, 61, 61)), -interfacemargin, -interfacemargin, interfacew + 2 * interfacemargin, interfaceh + 2* interfacemargin)

        for b in scrollButtons do
            b.Paint(g)
        for b in toggleButtons do
            b.Paint(g)
        for b in pmButtons do
            b.Paint(g)
        
        g.ResetTransform()
        

    override x.OnMouseDown e =
        base.OnMouseDown(e)
        
        doubleclick <- false

        if e.Button = MouseButtons.Left then
            clickpx <- e.X - scrollx
            clickpy <- e.Y - scrolly
            let ifaceclickx = e.X - x.Width + interfacew + interfacemargin
            let ifaceclicky = e.Y - interfacemargin

            for b in scrollButtons do
                if b.Contains(Point(ifaceclickx, ifaceclicky)) then
                    buttonclick <- true
                    action <- b.Action
                    b.Select()
                    x.Invalidate()

            for b in toggleButtons do
                if b.Contains(Point(ifaceclickx, ifaceclicky)) then
                    buttonclick <- true
                    action <- b.Action
                    
            for b in pmButtons do
                if b.Contains(Point(ifaceclickx, ifaceclicky)) then
                    buttonclick <- true
                    action <- b.Action
                    b.Select()
                    x.Invalidate()
            
            if buttonclick then continueAction.Start() else
                for p in planets do
                    if p.Rect.Contains(int(float32(clickpx)/zoom), int(float32(clickpy)/zoom)) then
                       planetclick <- true
                       if e.Clicks = 2 then
                           doubleclick <- true
                           planetclicked <- p
                            
                        
            if not planetclick && not buttonclick && not doubleclick then 
                newPlanet.Center <- PointF(float32 clickpx / zoom, float32 clickpy / zoom)
                clicked <- true

    override x.OnMouseMove e =
        base.OnMouseMove(e)
         
        if clicked then
            let a = float32(clickpx - e.Location.X + int scrollx) / zoom
            let b = float32(clickpy - e.Location.Y + int scrolly) / zoom
            
            newPlanet.NewRadius((min 800.f (sqrt( a**2.f + b**2.f )))) //800, valore empirico oltre il quale la massa va in overflow

            x.Invalidate()
        

    override x.OnMouseUp e =
        base.OnMouseUp(e)
        
        if planetclick then planetclick <- false

        if doubleclick then
            let dialog = new OpenFileDialog()
            if dialog.ShowDialog() = DialogResult.OK  then 
                printfn "Selected image: %s" dialog.FileName
                planetclicked.LoadImage(dialog.FileName)
                x.Invalidate()
                doubleclick <- false

        elif buttonclick then
            continueAction.Stop()
            for b in scrollButtons do b.Unselect()
            for b in pmButtons do b.Unselect()
            match action with
            | "UP" -> x.Scroll("U")
            | "DOWN" -> x.Scroll("D")
            | "LEFT" -> x.Scroll("L")
            | "RIGHT" -> x.Scroll("R") 
            | "StartStop" -> x.StartStop() 
            | "ToggleSquares" -> x.ToggleSquares()
            | "ToggleLines" -> x.ToggleLines()
            | "ToggleAcceleration" -> x.ToggleAcc()
            | "ZoomOut" -> x.Zoom("IN")
            | "ZoomIn" -> x.Zoom("OUT")
            | "FieldInterval-" -> x.modsquaresInterval("-")
            | "FieldInterval+" -> x.modsquaresInterval("+")
            | _ -> ()

            buttonclick <- false
            action <- "NONE"
        
        elif clicked then
            clicked <- false

            if newPlanet.Mass > 0.f then
                planets.Add(newPlanet)
                newPlanet <- new Planet(PointF(0.f,0.f))

                x.Invalidate()

    override x.OnResize e =
        base.OnResize(e)
        x.Invalidate()


/////////////// MAIN ///////////////

let form = new Form(Text="SpaceSimulator", BackColor = Color.Black, Width = 1366, Height = 768)
let space = new Flatlandia(Dock = DockStyle.Fill)

space.KeyDown.Add(fun e ->
    match e.KeyData with
    | Keys.W -> space.Scroll("U")
    | Keys.S -> space.Scroll("D")
    | Keys.A -> space.Scroll("L")
    | Keys.D -> space.Scroll("R")
    | Keys.X -> space.Zoom("IN")
    | Keys.Z -> space.Zoom("OUT")
    | Keys.Space -> space.StartStop()
    | Keys.Escape -> form.Close()
    | Keys.F1 -> space.ToggleLines()
    | Keys.F2 -> space.ToggleSquares()
    | Keys.F3 -> space.ToggleAcc()
    | Keys.N -> space.modsquaresInterval("-")
    | Keys.M -> space.modsquaresInterval("+")
    //
    | Keys.I -> space.PrintInfoPos()
    | Keys.O -> space.BigCrush()
    | _ -> ()
    )


form.Controls.Add(space)

//form.Show()

//while form.Created do 
//    if space.isMoving then 
//        space.eppurSiMuove()
//    Application.DoEvents() 
[<System.STAThread>]
Application.Run(form)