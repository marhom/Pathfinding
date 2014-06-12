using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AI_Project_2
{
    class World
    {
        private const int width = 80;
        private const int height = 60;

        public Agent activeAgent;
        public Texture2D nodeTexture;
        public Texture2D agentTexture;
        public Texture2D uiTexture;
        public Texture2D buttonTexture;
        public static Texture2D wallTexture;
        public mouseHandler mousehandler;
        public SpriteFont font;
        public SpriteFont displayFont;

        /* %%% storage containers %%% */
        public node[,] nodeArray;
        public LinkedList<Wall> wallList;
        public LinkedList<Button> buttonList;
        public static int totalNodes = 0;
        /* %%% debug UI %%% */
        public Rectangle debug;
        public Rectangle ui;
        public bool showUI;
        public bool showDebug;
        /* %%% pathfinding data members %%% */
        public LinkedList<node> path;
        public node curNode;
        public node startNode;
        public node destNode;
        public Vector2 destPosition;
        public bool nodeDest;
        public bool donePath;
        public bool showNodes;
        public bool wallMode;
        public bool clickMode;
        public bool startFound;
        public bool smoothingToggle;
        public bool impossible;
        public static bool drawOpened;
        public List<node> visited;
        public List<node> expanded;
        public List<node> adjacent;
        public Wall wallTemp;
        public Vector2 wallCorner, wallEnd;
        /* %%% debug %%% */
        public bool pathFound;
        public bool pauseMove;
        public node debugNode;
        public LinkedList<Wall> testList;
        public bool smoothTest;
        /* %%% deprecated %%% */
        public LinkedList<node> nodeGrid;


        public void initializeWorld(Texture2D agentSprite, Texture2D nodeSprite, Texture2D wallSprite, Texture2D uiSprite, Texture2D buttonSprite, SpriteFont font, SpriteFont displayFont, ref mouseHandler mousehandler)
        {
            nodeArray = new node[81, 61];
            this.mousehandler = mousehandler;
            nodeTexture = nodeSprite;
            uiTexture = uiSprite;
            agentTexture = agentSprite;
            buttonTexture = buttonSprite;
            this.displayFont = displayFont;
            activeAgent = new Agent(agentSprite);
            nodeGrid = new LinkedList<node>();
            path = new LinkedList<node>();
            wallList = new LinkedList<Wall>();
            buttonList = new LinkedList<Button>();
            this.font = font;
            pathFound = false;
            wallTexture = wallSprite;
            createWalls(wallSprite);
            generateNodeArray();
            destNode = null;
            showDebug = false;
            pauseMove = true;
            showNodes = false;
            startFound = true;
            clickMode = false;
            showUI = false;
            showDebug = false;
            impossible = false;
            smoothTest = true;
            drawOpened = false;
            wallCorner = new Vector2(-1, -1);
            debug = new Rectangle(0, 0, 175, 600);
            ui = new Rectangle(800 - uiTexture.Width, 0, uiTexture.Width, uiTexture.Height);
            testList = new LinkedList<Wall>();

            generateButtons();

        }
        public void generateButtons()
        {
            buttonList.AddFirst(new Button(new Rectangle(ui.X + 20, ui.Y + 20, ui.Width - 40, 75), buttonTexture, font, "Hide Nodes", 1, ref mousehandler));
            buttonList.AddFirst(new Button(new Rectangle(ui.X + 20, ui.Y + 40 + buttonTexture.Height, ui.Width - 40, 75), buttonTexture, font, "Select Path", 2, ref mousehandler));
            buttonList.AddFirst(new Button(new Rectangle(ui.X + 20, ui.Y + 60 + buttonTexture.Height * 2, ui.Width - 40, 75), buttonTexture, font, "A* without smoothing", 3, ref mousehandler));
            buttonList.AddFirst(new Button(new Rectangle(ui.X + 20, ui.Y + 80 + buttonTexture.Height * 3, ui.Width - 40, 75), buttonTexture, font, "Wall Mode", 4, ref mousehandler));
            buttonList.AddFirst(new Button(new Rectangle(ui.X + 20, ui.Y + 100 + buttonTexture.Height * 4, ui.Width - 40, 75), buttonTexture, font, "Show Opened Nodes", 5, ref mousehandler));

        }
        public void reinitAgent()
        {
            activeAgent = new Agent(agentTexture);
        }

        /* %%% createWalls %%%                      */
        /* Generates predefined walls in the World  */
        public void createWalls(Texture2D wallSprite)
        {
            //wallList.AddFirst(new Wall(wallSprite, new Rectangle(300, 200, 100, 50)));
            //wallList.AddFirst(new Wall(wallSprite, new Rectangle(600, 400, 29, 127)));
        }//createWalls

        /* %%% generateNodeArray %%%                */
        /* Generates an array-based network of nodes*/

        public void generateNodeArray()
        {
            for (int i = 0; i < 81; i++)
                for (int j = 0; j < 61; j++)
                {
                    nodeArray[i, j] = new node(new Vector2(i * 10, j * 10), nodeTexture, ++totalNodes, activeAgent.collisionRectangle, i, j);
                    foreach (Wall wall in wallList)
                        if (nodeArray[i, j].collisionRectangle.Intersects(wall.boundingRectangle))
                            nodeArray[i, j].activeNode = false;
                }
            for (int i = 0; i < 81; i++)
                for (int j = 0; j < 61; j++)
                {
                    if (i != 0 && j != 0)
                        nodeArray[i, j].adjacentNodes.AddFirst(new Point(i - 1, j - 1));
                    if (i != 0)
                        nodeArray[i, j].adjacentNodes.AddFirst(new Point(i - 1, j));
                    if (i != 0 && j != 60)
                        nodeArray[i, j].adjacentNodes.AddFirst(new Point(i - 1, j + 1));
                    if (i != 80 && j != 0)
                        nodeArray[i, j].adjacentNodes.AddFirst(new Point(i + 1, j - 1));
                    if (i != 80)
                        nodeArray[i, j].adjacentNodes.AddFirst(new Point(i + 1, j));
                    if (i != 80 && j != 60)
                        nodeArray[i, j].adjacentNodes.AddFirst(new Point(i + 1, j + 1));
                    if (j != 0)
                        nodeArray[i, j].adjacentNodes.AddFirst(new Point(i, j - 1));
                    if (j != 60)
                        nodeArray[i, j].adjacentNodes.AddFirst(new Point(i, j + 1));
                }
        }//generateNodeArray

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Wall wall in wallList)
                wall.Draw(spriteBatch);
            if (showNodes)
                foreach (node Node in nodeArray)
                    Node.Draw(spriteBatch);
            if (wallMode && wallTemp != null)
                wallTemp.Draw(spriteBatch);
            if (!clickMode && !wallMode)
                activeAgent.Draw(spriteBatch);
            DrawDebug(spriteBatch);
            DrawUI(spriteBatch);
            errorText(spriteBatch);
            if (testList.Count > 0)
                foreach (Wall wall in testList)
                    wall.Draw(spriteBatch);
            //if (pathFound)
            //debugText(spriteBatch);
            mousehandler.Draw(spriteBatch);

        }
        /// <summary>
        /// %%% updateWorld %%%                         
        /// Updates all contingent members of the World 
        ///</summary>
        public void updateWorld()
        {
            buttonUpdate();
            activeAgent.UpdateAgent();
            mousehandler.Update();
            checkClicked();
            if (curNode != null && destNode != null && smoothingToggle)
                smoothPath();
            if (!pathFound && destNode != null && destNode.activeNode && ((clickMode && startNode != null) || !clickMode))
                findPath();
            if (!impossible && pathFound && !pauseMove && startFound && !wallMode)
                movePath();

        }
        /// <summary>
        /// %%% findPath %%%                         
        /// Generates a path using the A* heuristic
        ///</summary>
        public void findPath()
        {
            impossible = false;
            visited = new List<node>();
            expanded = new List<node>();
            adjacent = new List<node>();

            /* %%% CHANGE TO START NODE ALG %%% */
            if (!clickMode)
            {
                expanded.Add(activeAgent.findClosest(nodeArray));
                setOpened(activeAgent.findClosest(nodeArray));

            }
            else
            {
                expanded.Add(startNode);
                setOpened(startNode);
            }
            while (!visited.Contains(destNode) && expanded.Count > 0 && !impossible)
            {
                curNode = expanded[0];
                /* loop through opened nodes looking for lowest scoring one */
                for (int i = 1; i < expanded.Count; i++)
                    if (nodeArray[expanded[i].gridPosition.X, expanded[i].gridPosition.Y].getScore()
                        <= nodeArray[curNode.gridPosition.X, curNode.gridPosition.Y].getScore())
                        curNode = expanded[i];
                /*once lowest scorer is found, go to it*/
                expanded.Remove(curNode);
                visited.Add(curNode);
                /*look at neighboring nodes to find the lowest scoring node off of that node*/
                adjacent = checkAdjacent(curNode, destNode);
                /*update parents for lower-cost paths*/
                for (int i = 0; i < adjacent.Count; i++)

                    if (nodeArray[adjacent[i].gridPosition.X, adjacent[i].gridPosition.Y].getScore() > nodeArray[curNode.gridPosition.X, curNode.gridPosition.Y].getScore())
                    {
                        nodeArray[adjacent[i].gridPosition.X, adjacent[i].gridPosition.Y].parent = curNode;
                        nodeArray[adjacent[i].gridPosition.X, adjacent[i].gridPosition.Y].setScore(curNode, destNode);
                    }
            }
            if (visited.Contains(destNode))
            {
                makePath(curNode);
                pathFound = true;
                TextWriter textwriter = new StreamWriter("consoleDump.txt");
                foreach (node Node in visited)
                {
                    textwriter.WriteLine(Node.ToString());
                }
                textwriter.Close();
            }
            else
            {
                impossible = true;
                path = new LinkedList<node>();
                destNode = null;
            }

        }
        /// <summary>
        /// %%% makePath %%%                         
        /// Reorganizes the path and adds start/end virtual nodes 
        ///</summary>
        public void makePath(node currentNode)
        {
            path = new LinkedList<node>();
            if (!clickMode)
                path.AddFirst(new node(destPosition, nodeTexture, -1, activeAgent.collisionRectangle, -1, -1));
            do
            {
                path.AddFirst(currentNode);
                setSelected(currentNode);
                currentNode = currentNode.parent;
            }
            while (currentNode.parent != null);

            //if(path.First().center != activeAgent.center)
            //    path.AddFirst(new node(activeAgent.center, nodeTexture, -1, activeAgent.collisionRectangle, -1, -1, Color.Green));
            //path.AddLast(new node(destPosition, nodeTexture, -1, activeAgent.collisionRectangle, -1, -1, Color.Red));
            if (!clickMode)
                path.AddFirst(new node(activeAgent.center, nodeTexture, -1, activeAgent.collisionRectangle, -1, -1));
            donePath = false;

        }
        /// <summary>
        /// %%% movePath %%%                         
        /// Moves the agent along the path
        ///</summary>
        public void movePath()
        {
            if (path.Count == 0)
            {
                donePath = true;
                activeAgent.clearTarget();
                pauseMove = true;
            }
            else if (donePath == false && activeAgent.targetNode == null)
            {
                //activeAgent.center = path.First().center;
                //path.RemoveFirst();
                activeAgent.targetNode = path.First();
                path.RemoveFirst();
                pauseMove = true;
            }
            else if (activeAgent.agentArrived())
            {
                activeAgent.targetNode = path.First();
                path.RemoveFirst();
                pauseMove = true;
            }

        }
        /// <summary>
        /// %%% checkAdjacent %%%                         
        /// Open the nearby nodes for findPath
        ///</summary>    
        public List<node> checkAdjacent(node curNode, node destNode)
        {
            List<node> temp = new List<node>();
            foreach (Point adjNode in curNode.adjacentNodes)
                if (visited.Contains(nodeArray[adjNode.X, adjNode.Y]) == false && nodeArray[adjNode.X, adjNode.Y].activeNode == true)
                {
                    if (expanded.Contains(nodeArray[adjNode.X, adjNode.Y]))
                        temp.Add(nodeArray[adjNode.X, adjNode.Y]);
                    else
                    {
                        nodeArray[adjNode.X, adjNode.Y].parent = curNode;
                        nodeArray[adjNode.X, adjNode.Y].setScore(curNode, destNode);
                        expanded.Add(nodeArray[adjNode.X, adjNode.Y]);
                        setOpened(nodeArray[adjNode.X, adjNode.Y]);
                    }
                }
            return temp;
        }
        /// <summary>
        /// %%% findClosestToMouse %%%                         
        /// Get a position for the virtual final node
        ///</summary>
        public node findClosestToMouse()
        {
            node temp = null;
            float distance = 0;
            foreach (node Node in nodeArray)
            {
                if (temp == null)
                {
                    temp = Node;
                    distance = (mousehandler.position - Node.center).Length();
                }
                else
                {
                    if (distance > (mousehandler.position - Node.center).Length())
                    {
                        temp = Node;
                        distance = (mousehandler.position - Node.center).Length();
                    }
                }
            }

            return temp;
        }
        /// <summary>
        /// %%% checkClicked %%%                         
        /// Handles all mouse logic
        ///</summary>
        public void checkClicked()
        {

            if (showUI)
            {
                if (mousehandler.getState().LeftButton == ButtonState.Pressed && mousehandler.getPrevState().LeftButton == ButtonState.Released)
                {
                    foreach (Button button in buttonList)
                    {
                        if (mousehandler.overObject(button))
                        {
                            if (button.id == 2)
                            {
                                clearStart();
                                clearSelected();
                                destNode = null;
                                activeAgent.clearTarget();
                            }
                            else if (button.id == 4)
                            {
                                clearStart();
                                clearSelected();
                                destNode = null;
                                activeAgent.clearTarget();
                                pathFound = false;
                            }
                            button.toggle = !button.toggle;
                        }
                    }
                }
            }//UI BUTTON ROUTINE

            // %%% WALL MODE ROUTINE %%%
            else if (wallMode)
            {
                if (wallCorner != new Vector2(-1, -1))
                {
                    if (mousehandler.getState().RightButton == ButtonState.Pressed && mousehandler.getPrevState().RightButton == ButtonState.Released)
                    {
                        wallCorner = new Vector2(-1, -1);
                        wallTemp = null;
                    }
                    else
                    {
                        wallEnd = new Vector2((mousehandler.position.X < wallCorner.X ? wallCorner.X : mousehandler.position.X), (mousehandler.position.Y < wallCorner.Y ? wallCorner.Y : mousehandler.position.Y));
                        wallTemp = new Wall(wallTexture, new Rectangle((int)wallCorner.X, (int)wallCorner.Y, (int)wallEnd.X - (int)wallCorner.X, (int)wallEnd.Y - (int)wallCorner.Y));
                    }
                }
                if ((mousehandler.getState().LeftButton == ButtonState.Pressed && mousehandler.getPrevState().LeftButton == ButtonState.Released))
                {
                    if (wallCorner == new Vector2(-1, -1))
                        wallCorner = mousehandler.position;
                    else
                    {
                        addWalls();
                        wallEnd = new Vector2(-1, -1);
                    }
                }
                if ((mousehandler.getState().RightButton == ButtonState.Pressed && mousehandler.getPrevState().RightButton == ButtonState.Released) && wallEnd == new Vector2(-1, -1))
                {
                    LinkedList<Wall> temp = new LinkedList<Wall>();
                    foreach (Wall wall in wallList)
                        if (mousehandler.overObject(wall))
                            temp.AddFirst(wall);
                    if (temp.Count > 0)
                    {
                        foreach (Wall wall in temp)
                            wallList.Remove(wall);

                    }
                    updateGrid();
                }
            }
            // %%% SELECTION %%%
            else if (mousehandler.getState().LeftButton == ButtonState.Pressed && mousehandler.getPrevState().LeftButton == ButtonState.Released)
            {
                pathFound = false;
                impossible = false;
                activeAgent.clearTarget();
                clearSelected();
                destNode = findClosestToMouse();
                destPosition = mousehandler.position;
            }
            else if (!clickMode && mousehandler.getState().RightButton == ButtonState.Pressed && mousehandler.getPrevState().RightButton == ButtonState.Released)
            {
                if (debugNode != null)
                    nodeArray[debugNode.gridPosition.X, debugNode.gridPosition.Y].color = Color.Black;

                debugNode = findClosestToMouse();
                nodeArray[debugNode.gridPosition.X, debugNode.gridPosition.Y].color = Color.Blue;
            }

            // %%% CLICKMODE ROUTINE %%%
            else if (clickMode && mousehandler.getState().RightButton == ButtonState.Pressed && mousehandler.getPrevState().RightButton == ButtonState.Released)
            {
                clearStart();
                node tempNode = findClosestToMouse();
                if (tempNode == startNode && startFound)
                {
                    startFound = false;
                    clearSelected();
                    clearStart();
                    pathFound = false;
                }
                else
                {
                    startNode = tempNode;
                    startFound = true;
                    if (path.Count > 0)
                    {
                        clearSelected();
                        destNode = path.Last();
                        path.Clear();
                        findPath();
                    }
                    nodeArray[startNode.gridPosition.X, startNode.gridPosition.Y].color = Color.Green;
                }
            }
        }
        /// <summary>
        /// %%% debugText %%%                         
        /// deprecated
        ///</summary>
        public void debugText(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "dest node " + destNode.nodeNumber + "\ntarget node " + activeAgent.targetNode.nodeNumber + "\n", new Vector2(200, 200), Color.Blue);
        }
        /// <summary>
        /// %%% setSelected / setOpened / clearSelected / clearOpened / clearStart %%%                         
        /// Sets selected / opened flag on a node or clears all the nodes flags
        ///</summary>
        public void setSelected(node activeNode)
        {
            nodeArray[activeNode.gridPosition.X, activeNode.gridPosition.Y].isSelected = true;
        }
        public void setOpened(node activeNode)
        {
            nodeArray[activeNode.gridPosition.X, activeNode.gridPosition.Y].isOpened = true;
        }
        public void clearSelected()
        {
            foreach (node Node in nodeArray)
            {
                Node.isSelected = false;
                Node.parent = null;
                Node.isOpened = false;

            }
        }
        public void clearOpened()
        {
            foreach (node Node in nodeArray)
            {

            }
        }
        public void clearStart()
        {
            foreach (node Node in nodeArray)
            {
                Node.color = Color.Black;
            }
        }
        public void DrawDebug(SpriteBatch spriteBatch)
        {
            if (showDebug)
            {
                spriteBatch.Draw(wallTexture, debug, Color.Black);
                string pathInfo = "\nPath Nodes:\n";
                if (path.Count != 0)
                    foreach (node Node in path)
                        pathInfo = pathInfo + Node.nodeNumber + " " + Node.center + "\n";
                spriteBatch.DrawString(font, "DEBUG\n\n\n\nTarget:" + (activeAgent.targetNode == null ? -2 : activeAgent.targetNode.nodeNumber) + "\nAgent Position:\n " + activeAgent.position + "\nAgent Center:\n" + activeAgent.center + "\nPathCount: " + path.Count() + "\nNode info:\n" + (debugNode != null ? debugNode.center.ToString() + debugNode.isOpened : ""), new Vector2(5, 5), Color.White);
            }
        }
        public void DrawUI(SpriteBatch spriteBatch)
        {
            if (showUI)
            {
                spriteBatch.Draw(uiTexture, ui, Color.White);
                foreach (Button button in buttonList)
                {
                    button.Draw(spriteBatch);
                }

            }
        }
        public void buttonUpdate()
        {
            foreach (Button button in buttonList)
            {
                if (button.id == 1)
                {
                    showNodes = button.toggle;
                    if (!button.toggle)
                        button.text = "Show Nodes";
                    else
                        button.text = "Hide Nodes";
                }
                if (button.id == 2)
                {
                    clickMode = button.toggle;
                    startFound = !button.toggle;
                    if (button.toggle)
                        button.text = "Use Agent";
                    else
                        button.text = "Select Path";
                }
                if (button.id == 3)
                {
                    smoothingToggle = button.toggle;
                    if (button.toggle)
                        button.text = "A* without smoothing";
                    else
                        button.text = "A* with smoothing";
                }
                if (button.id == 4)
                {
                    wallMode = button.toggle;
                    if (button.toggle)
                        button.text = "Path Mode";
                    else
                        button.text = "Wall Mode";
                }
                if (button.id == 5)
                {
                    drawOpened = button.toggle;
                    if (!button.toggle)
                        button.text = "Show Opened Nodes";
                    else
                        button.text = "Hide Opened Nodes";
                }
            }
        }
        public void updateGrid()
        {
            foreach (node Node in nodeArray)
            {
                bool updatedThisPass = false;
                foreach (Wall wall in wallList)
                {
                    if (Node.collisionRectangle.Intersects(wall.boundingRectangle))
                    {
                        Node.activeNode = false;
                        updatedThisPass = true;
                    }
                    else if (!updatedThisPass)
                        Node.activeNode = true;
                }
            }
        }
        public void addWalls()
        {
            wallList.AddFirst(wallTemp);
            wallTemp = null;
            wallCorner = new Vector2(-1, -1);
            wallEnd = new Vector2(-1, -1);
            updateGrid();
        }
        public void errorText(SpriteBatch spriteBatch)
        {
            if (impossible)
                spriteBatch.DrawString(displayFont, "Path is impossible.", new Vector2((800 - displayFont.MeasureString("Path is impossible.").X) / 2, 20), Color.Red);
            if (!smoothTest)
                spriteBatch.DrawString(displayFont, "CANNOT SMOOTH", new Vector2((800 - displayFont.MeasureString("CANNOT SMOOTH").X) / 2, 20), Color.Red);

        }
        public void smoothPath()
        {
            LinkedList<node> pathTemp = new LinkedList<node>(path);
            //LinkedListNode<node> pathNode = new LinkedListNode<node>(;
            bool smoothingDone = false;
            if (path.Count > 1)
                if (testAdvance(path.First(), path.Last()))
                    return;
                else
                {
                    LinkedListNode<node> temp = new LinkedListNode<node>(null);
                    temp = pathTemp.First;
                    while (true)
                    {
                        temp = pathTemp.First;
                        if (temp.Next == null || temp.Next.Next == null)
                            break;
                        if (testAdvance(temp.Value, temp.Next.Value, temp.Next.Next.Value))
                        {
                            pathTemp.Remove(temp.Next);
                            smoothingDone = false;
                        }
                        else
                            pathTemp.Remove(temp);
                        if (pathTemp.Count < 3)
                            break;
                    }
                    //bool noChange = false;
                    //while (!noChange)
                    //{

                    //    foreach (node Node in pathTemp)
                    //    {
                    //        noChange = true;
                    //        if (pathTemp.First.Next.Value == pathTemp.Last.Value)
                    //            break;
                    //        if (testAdvance(pathTemp.First.Value, pathTemp.First.Next.Value, pathTemp.First.Next.Next.Value))
                    //        {
                    //            pathTemp.Remove(pathTemp.First.Next.Value);
                    //            noChange = false;
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            pathTemp.Remove(pathTemp.First);
                    //            noChange = false;
                    //            break;
                    //        }
                    //    }
                    //}
                }
            //}


        }
        public bool testAdvance(node start, node skip, node end)
        {
            Agent testAgent = new Agent(agentTexture);
            smoothTest = true;
            bool collide = false;
            testAgent.position = start.center - testAgent.centerPoint;
            testAgent.targetNode = end;
            while (collide == false && !testAgent.agentArrived())
            {
                testAgent.UpdateAgent();
                foreach (Wall wall in wallList)
                {
                    if (testAgent.collisionRectangle.Intersects(wall.boundingRectangle))
                    {
                        collide = true;
                        //smoothTest = false;
                        return false;
                    }
                }
            }
            path.Remove(skip);
            nodeArray[skip.gridPosition.X, skip.gridPosition.Y].isSelected = false;
            return true;
        }
        public bool testAdvance(node start, node end)
        {
            Agent testAgent = new Agent(agentTexture);
            smoothTest = true;
            bool collide = false;
            testAgent.position = start.center - testAgent.centerPoint;
            testAgent.targetNode = end;
            while (collide == false && !testAgent.agentArrived())
            {
                foreach (Wall wall in wallList)
                {
                    if (testAgent.collisionRectangle.Intersects(wall.boundingRectangle))
                    {
                        collide = true;
                        //smoothTest = false;
                        return false;
                    }
                }
                testAgent.UpdateAgent();
            }
            clearSelected();
            path.Clear();
            path.AddFirst(end);
            path.AddFirst(start);
            if (start.nodeNumber > 0)
                nodeArray[start.gridPosition.X, start.gridPosition.Y].isSelected = true;
            if (end.nodeNumber > 0)
                nodeArray[end.gridPosition.X, end.gridPosition.Y].isSelected = true;
            return true;
        }
        public bool nodeToNode(node start, node end)
        {
            Vector2 headingAngle = new Vector2();
            testList.Clear();
            headingAngle = start.center - end.center;
            Rectangle collisionRectangle = activeAgent.collisionRectangle;
            float angle = Vector2.Dot(new Vector2(1, 0), headingAngle);
            //do{
            collisionRectangle.X = (int)(start.center.X + (start.center.X >= end.center.X ? -1 : 1) * Math.Cos(angle) * collisionRectangle.Width + collisionRectangle.Width / 2);
            collisionRectangle.Y = (int)(start.center.Y + (start.center.Y >= end.center.Y ? -1 : 1) * Math.Sin(angle) * collisionRectangle.Height + (angle > 45 && angle < 45 ? -1 : 1) * collisionRectangle.Height / 2);
            testList.AddFirst(new Wall(wallTexture, collisionRectangle));
            //}while(collisionRectangle
            return true;
        }
    }//world
}//namespace

/*%%% Old code, utilizing node-to-node pathfinding ONLY %%% */
//foreach (node Node in nodeGrid)//nodeGrid
//{

//    if (mousehandler.overObject(Node) && mousehandler.getState().LeftButton == ButtonState.Pressed && mousehandler.getPrevState().LeftButton == ButtonState.Released)
//    {
//        Node.isSelected = !Node.isSelected;
//        if (Node.isSelected)
//        {
//            //activeAgent.setTarget(Node);
//            destNode = Node;
//            findPath();
//            activeAgent.setTarget(path.First());
//        }
//        else
//        {
//            //activeAgent.clearTarget();
//            destNode = null;
//            path = new LinkedList<node>();
//        }

//    }
//    else if (Node.isSelected && activeAgent.agentArrived())
//    {
//        Node.isSelected = false;
//        //activeAgent.clearTarget();
//        destNode = null;
//        path = new LinkedList<node>();
//    }
//}
/* %%% populateNodes %%%                    */
/* Generates a list-based network of nodes  */
//public void populateNodes(int x, int y)
//{
//    if (nodeGrid.First == null)
//        /*occlusion check*/
//        nodeGrid.AddFirst(new node(new Vector2(x, y), nodeTexture, ++totalNodes));
//    else
//    {
//        if (y <= 600 && y >= 0 && x <= 800 && x >= 0)
//            nodeGrid.AddAfter(new node(new Vector2(x, y), nodeTexture, ++totalNodes));
//    }
//    nodeGrid.AddFirst(new node(new Vector2(500, 100), nodeTexture, ++totalNodes));
//    nodeGrid.AddFirst(new node(new Vector2(700, 200), nodeTexture, ++totalNodes));

//}
//public void movePath()
//{

//    if (activeAgent.agentArrived())
//    {
//        if (donePath)
//        {
//            donePath = false;
//            activeAgent.clearTarget();
//        }
//        else
//            if (activeAgent.targetNode == destNode)
//            {
//                //activeAgent.setTarget(new node(destPosition, nodeTexture, 0, activeAgent.collisionRectangle));
//                donePath = true;
//            }
//    }
//    else if(path.Count() != 0)
//    {
//        activeAgent.setTarget(path.First());
//         path.RemoveFirst();
//    }
//}