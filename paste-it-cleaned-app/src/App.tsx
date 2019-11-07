import React from "react"
import { BrowserRouter, Switch, Route, Link } from "react-router-dom"

import { createMuiTheme, createStyles, ThemeProvider, withStyles, WithStyles } from "@material-ui/core/styles"
import CssBaseline from "@material-ui/core/CssBaseline"
import Hidden from "@material-ui/core/Hidden"

import Navigator from "./components/Navigator"
import Content from "./components/Content"
import Header from "./components/Header"
import AccountInformation from "./views/account/information/AccountInformation"
import Footer from "./components/Footer"

let theme = createMuiTheme({
  palette: {
    primary: {
      light: "#63ccff",
      main: "#009be5",
      dark: "#006db3"
    }
  },
  typography: {
    h5: {
      fontWeight: 500,
      fontSize: 26,
      letterSpacing: 0.5
    }
  },
  shape: {
    borderRadius: 8
  },
  props: {
    MuiTab: {
      disableRipple: true
    }
  },
  mixins: {
    toolbar: {
      minHeight: 48
    }
  }
})

theme = {
  ...theme,
  overrides: {
    MuiDrawer: {
      paper: {
        backgroundColor: "#18202c"
      }
    },
    MuiButton: {
      label: {
        textTransform: "none"
      },
      contained: {
        boxShadow: "none",
        "&:active": {
          boxShadow: "none"
        }
      }
    },
    MuiTabs: {
      root: {
        marginLeft: theme.spacing(1)
      },
      indicator: {
        height: 3,
        borderTopLeftRadius: 3,
        borderTopRightRadius: 3,
        backgroundColor: theme.palette.common.white
      }
    },
    MuiTab: {
      root: {
        textTransform: "none",
        margin: "0 16px",
        minWidth: 0,
        padding: 0,
        [theme.breakpoints.up("md")]: {
          padding: 0,
          minWidth: 0
        }
      }
    },
    MuiIconButton: {
      root: {
        padding: theme.spacing(1)
      }
    },
    MuiTooltip: {
      tooltip: {
        borderRadius: 4
      }
    },
    MuiDivider: {
      root: {
        backgroundColor: "#404854"
      }
    },
    MuiListItemText: {
      primary: {
        fontWeight: theme.typography.fontWeightMedium
      }
    },
    MuiListItemIcon: {
      root: {
        color: "inherit",
        marginRight: 0,
        "& svg": {
          fontSize: 20
        }
      }
    },
    MuiAvatar: {
      root: {
        width: 32,
        height: 32
      }
    }
  }
}

const drawerWidth = 256

const styles = createStyles({
  root: {
    display: "flex",
    minHeight: "100vh"
  },
  drawer: {
    [theme.breakpoints.up("sm")]: {
      width: drawerWidth,
      flexShrink: 0
    }
  },
  app: {
    flex: 1,
    display: "flex",
    flexDirection: "column"
  },
  main: {
    flex: 1,
    padding: theme.spacing(6, 4),
    background: "#eaeff1"
  }
})

export interface AppProps extends WithStyles<typeof styles> {}

export interface AppState {
  mobileOpen: boolean
}

class App extends React.Component<AppProps, AppState> {
  constructor(props: AppProps) {
    super(props)
    this.state = { mobileOpen: false }
  }

  render() {
    const { classes } = this.props

    const handleDrawerToggle = () => {
      this.setState({ mobileOpen: !this.state.mobileOpen })
    }

    return (
      <BrowserRouter>
        <ThemeProvider theme={theme}>
          <div className={classes.root}>
            <CssBaseline />

            <nav className={classes.drawer}>
              <Hidden smUp implementation="js">
                <Navigator current={new Date().toLocaleTimeString()} PaperProps={{ style: { width: drawerWidth } }} variant="temporary" open={this.state.mobileOpen} onClose={handleDrawerToggle} />
              </Hidden>
              <Hidden xsDown implementation="css">
                <Navigator current={new Date().toLocaleTimeString()} PaperProps={{ style: { width: drawerWidth } }} />
              </Hidden>
            </nav>

            <div className={classes.app}>
              <Header onDrawerToggle={handleDrawerToggle} />
              <main className={classes.main}>
                <Switch>
                  <Route exact path="/">
                    <Content />
                  </Route>
                  <Route path="/account">
                    <AccountInformation />
                  </Route>
                  <Route path="/dashboard">
                    <Content />
                  </Route>
                </Switch>
              </main>
              <Footer />
            </div>
          </div>
        </ThemeProvider>
      </BrowserRouter>
    )
  }
}

export default withStyles(styles)(App)
