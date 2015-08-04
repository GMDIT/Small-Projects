// Game of life by Jammy, 09-04-2012
// Regole:
//-una cella morta con _esattamente_ 3 vicini NASCE
//-una cella viva con 2 o 3 vicini sopravvive
//-una cella con 1 o più di 3 vicini muore
//
//Stati:
//- 0: morta
//- 1: nascente
//- 2: viva
//- 3: morente

open System.Windows.Forms
open System.Drawing
open System.Drawing.Imaging
open System

type TheWorld(ww : int, wh : int, c:int, s:int, i:int) as x=
    inherit UserControl()

    let mutable width = ww //colonne matrice
    let mutable height = wh // righe matrice
    let mutable killemall = false //maybe unused
    let mutable circlecell = false
    let cellsize = c //dimensione cella
    let span = s //span tra le celle
    let mutable generation = 0 //# generazioni
    let timer = new Timer(Interval=i)
    let randomNumberGenerator = new Random()
    let mutable running = false
    let mutable clicked = false
    let mutable oldcell = 0, 0
    let mutable showhud = true
    let mutable info = null //info hud
    let mutable info2 = null //info2 hud
    let font = new Font(FontFamily.Families.[2], 12.0F) //font hud
    let mutable hudrect = new Rectangle()
    
   
    // i è l'altezza della matrice (righe), j è la larghezza (colonne)
    let mutable matrix = Array2D.create height width 0
   
                //morta                               nascente                                              morente                                                 viva 
    let brush = [| new SolidBrush(Color.GhostWhite) ; new SolidBrush(Color.FromArgb(50, Color.LimeGreen)); new SolidBrush(Color.FromArgb(150, Color.LimeGreen));new SolidBrush(Color.Lime) |]


    do //random init
        x.SetStyle(ControlStyles.OptimizedDoubleBuffer, true)

        timer.Tick.Add(fun _ ->
            x.Calculate()
            if showhud then x.Invalidate(hudrect)
        ) 
       
        for i in 0 .. 1 .. height-1 do
            for j in 0 .. 1 .. width-1 do
                matrix.[i,j] <- randomNumberGenerator.Next(4)

            

    member x.StartStop() = 
        if running then 
            timer.Stop()
            running <- false
            " ||  PAUSE"
        else
            timer.Start()
            running <- true
            " |>  RUNNING"

    //override x.OnPaintBackground e = ()
    override x.OnPaint e =
        let g = e.Graphics
        
        for i in 0 .. height-1 do
            for j in 0 .. width-1 do
                let rect = new Rectangle(j*cellsize, i*cellsize, cellsize-span, cellsize-span)
                if circlecell then g.FillEllipse(brush.[matrix.[i,j]], rect) else g.FillRectangle(brush.[matrix.[i,j]], rect)
                if matrix.[i,j] > 1 then
                    if circlecell then g.DrawEllipse(Pens.Black, rect ) else g.DrawRectangle(Pens.Black, rect )
                
        
        if showhud then x.PaintHud(g)      

    member x.Calculate() =
        let mutable l, r, t, b, count = 0, 0, 0, 0, 0
        generation <- generation + 1
        for i in 0 .. height-1 do
            for j in 0 .. width-1 do
                let rect = new Rectangle(j*cellsize, i*cellsize, cellsize, cellsize)
                if matrix.[i,j] = 1 then 
                    matrix.[i,j] <- 3
                    x.Invalidate(rect)
                if matrix.[i,j] = 2 then 
                    matrix.[i,j] <- 0
                    x.Invalidate(rect)

        for i in 0 .. height-1 do
            t <- (i + height - 1) % height
            b <- (i + height + 1) % height
            for j in 0 .. width-1 do
                let rect = new Rectangle(j*cellsize, i*cellsize, cellsize, cellsize)
                l <- (j + width - 1) % width
                r <- (j + width + 1) % width
                count <- 0
                if matrix.[t,l] > 1 then count <- count + 1
                if matrix.[t,j] > 1 then count <- count + 1
                if matrix.[t,r] > 1 then count <- count + 1
                if matrix.[i,l] > 1 then count <- count + 1
                if matrix.[i,r] > 1 then count <- count + 1
                if matrix.[b,l] > 1 then count <- count + 1
                if matrix.[b,j] > 1 then count <- count + 1
                if matrix.[b,r] > 1 then count <- count + 1

                if matrix.[i,j] = 3 && (count < 2 || count > 3) then 
                    matrix.[i,j] <- 2
                    x.Invalidate(rect)
                if matrix.[i,j] = 0 && count = 3 then 
                    matrix.[i,j] <- 1
                    x.Invalidate(rect)
    
    //member x.theKingJustice() =
        
                //if matrix.[i,j] = 1 || matrix.[i,j] = 2 then matrix.[i,j] <- (matrix.[i,j] + 2) % 4

//    override x.OnKeyUp e =
//        match e.KeyData with
//        | Keys.P | Keys.Space ->
//          x.StartStop() |> ignore
//        | _ -> ()

    member x.PaintHud(g:Graphics) =
        info <- sprintf "Generation %d" generation
        info2 <- sprintf "Speed : %1.1f" (1000.f / single(timer.Interval))
      
        let sz = g.MeasureString(info, font)
        let left = (single(x.Width) - sz.Width) / 2.f 
        let top = (single(x.Height) - 2.f * sz.Height) / 2.f
        let sz2 = g.MeasureString(info2, font)
        let left2 = (single(x.Width) - sz2.Width) / 2.f 
        let top2 = (single(x.Height) - sz2.Height - sz.Height) / 2.f 
        hudrect <- new Rectangle(int(left), int(top), int(max sz.Width  sz2.Width), int(sz.Height + sz2.Height))
               
       

        g.FillRectangle(new SolidBrush(Color.FromArgb(240, Color.GhostWhite)), hudrect)
        //g.FillEllipse(Brushes.Red, rect)
        
        //g.DrawRectangle(Pens.GhostWhite, rect)
        g.DrawString(info, font, Brushes.Snow, left + 1.f, top + 1.f)
        g.DrawString(info, font, Brushes.Black, left, top)
        g.DrawString(info2, font, Brushes.Snow, left2 + 1.f, top2 + sz.Height + 1.f)
        g.DrawString(info2, font, Brushes.Black, left2, top2 + sz.Height)

//        // per contrallare se l'hud è centrato
//        g.DrawLine(Pens.Red, new Point(0,0), new Point(x.Width, x.Height))
//        g.DrawLine(Pens.Red, new Point(x.Width,0), new Point(0, x.Height))


    override x.OnMouseDown e =
        clicked <- true
        timer.Stop()
        oldcell <- -1, -1
        let cy, cx = e.Location.Y / cellsize, e.Location.X/cellsize
        if cy >= 0 && cx >= 0 && cy < height && cx < width && (cy, cx) <> oldcell then
            matrix.[cy, cx] <- (matrix.[cy, cx] + 1) % 2
            oldcell <- cy, cx
            x.Invalidate(new Rectangle(cx * cellsize, cy*cellsize, cellsize, cellsize))
    
    override x.OnMouseMove e =
        if clicked then
            let cy, cx = e.Location.Y / cellsize, e.Location.X/cellsize
            if cy >= 0 && cx >= 0 && cy < height && cx < width && (cy, cx) <> oldcell then
                matrix.[cy, cx] <- (matrix.[cy, cx] + 1) % 2
                oldcell <- cy, cx
                x.Invalidate(new Rectangle(cx * cellsize, cy*cellsize, cellsize, cellsize))

    override x.OnMouseUp e =
        clicked <- false
        if running then timer.Start()

    member x.Clear() =
        timer.Stop()
        for i in 0 .. height-1 do
            for j in 0 .. width-1 do
                matrix.[i,j] <- 0
        generation <- 0
        x.Invalidate()
        running <- false
    
    member x.Random() =
        for i in 0 .. 1 .. height-1 do
            for j in 0 .. 1 .. width-1 do
                matrix.[i,j] <- randomNumberGenerator.Next(4)
        generation <- 0
        x.Invalidate()
    
    member x.SpeedUp() = 
        timer.Stop()
        if timer.Interval > 10 then timer.Interval <- timer.Interval - 10
        if showhud then x.Invalidate(hudrect)
        if running then timer.Start()
        

    member x.SpeedDown() =
        timer.Stop()
        timer.Interval <- timer.Interval + 10
        if showhud then x.Invalidate(hudrect)
        if running then timer.Start()

    member x.ToggleHud() =
        if showhud then showhud <- false
        else showhud <- true
        x.Invalidate(hudrect)

    member x.ToggleShape() =
        if circlecell then circlecell <- false else circlecell <- true
        x.Invalidate()
                 
    member x.GetPixelSize(d) =
        match d with
        | w -> (cellsize * width + 2*cellsize)
        | h -> (cellsize * height + 2 * cellsize + 22)

    member x.WW 
        with get() = width 
        and set(v) = width <- v
    member x.WH
        with get() = height
        and set(v) = height <- v


let w, h = 120 , 100
let c, s = 10, 2
let i = 1000
let f = new Form(Text="GameOfLife || PAUSE", TopMost=true)
let theworld = new TheWorld(w, h, c, s, i, Dock=DockStyle.Fill)

//f.Width <- c*w + 2*c
f.Height <-c*h + 2*c + 22
f.Width <- theworld.GetPixelSize("w")
//f.Height <- theworld.GetPixelSize("h")

printfn  "%d %d" f.Width, f.Height

theworld.KeyDown.Add(fun e ->
    match e.KeyData with
    | Keys.Space -> 
        let status = theworld.StartStop()
        let title = sprintf "GameOfLife  %s" status
        f.Text <- title 
    | Keys.C ->
        theworld.Clear()
        f.Text <- "GameOfLife  X CLEARED"
    | Keys.W ->
        theworld.SpeedUp()
    | Keys.S ->
        theworld.SpeedDown()
    | Keys.R ->
        theworld.Random()
    | Keys.H ->
        theworld.ToggleHud()
    | Keys.F ->
        theworld.ToggleShape()
    | _ -> ()
)
f.Controls.Add(theworld)
f.Show()