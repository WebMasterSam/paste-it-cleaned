import React, { ReactNode } from "react"
import AppBar from "@material-ui/core/AppBar"
import Avatar from "@material-ui/core/Avatar"
import Grid from "@material-ui/core/Grid"
import Hidden from "@material-ui/core/Hidden"
import IconButton from "@material-ui/core/IconButton"
import MenuIcon from "@material-ui/icons/Menu"
import NotificationsIcon from "@material-ui/icons/Notifications"
import Toolbar from "@material-ui/core/Toolbar"
import Tooltip from "@material-ui/core/Tooltip"
import Typography from "@material-ui/core/Typography"

import "./HeaderBase.less"

interface HeaderBaseProps {
  onDrawerToggle: () => void
  tabs: ReactNode
  title: ReactNode
}

class HeaderBase extends React.Component<HeaderBaseProps> {
  render() {
    const { onDrawerToggle } = this.props

    return (
      <React.Fragment>
        <AppBar color="primary" position="sticky" elevation={0}>
          <Toolbar>
            <Grid container spacing={1} alignItems="center">
              <Hidden smUp>
                <Grid item>
                  <IconButton color="inherit" aria-label="open drawer" onClick={onDrawerToggle} className="menu-button">
                    <MenuIcon />
                  </IconButton>
                </Grid>
              </Hidden>
              <Grid item xs />
              <Grid item>
                <Tooltip title="Notifications">
                  <IconButton color="inherit">
                    <NotificationsIcon />
                  </IconButton>
                </Tooltip>
              </Grid>
              <Grid item>
                <IconButton color="inherit" className="icon-button-avatar">
                  <Avatar src="/static/images/avatar/1.jpg" alt="My Avatar" />
                </IconButton>
              </Grid>
            </Grid>
          </Toolbar>
        </AppBar>
        <AppBar component="div" className="secondary-bar" color="primary" position="static" elevation={0}>
          <Toolbar>
            <Grid container alignItems="center" spacing={1}>
              <Grid item xs>
                <Typography color="inherit" variant="h5" component="h1">
                  {this.props.title}
                </Typography>
              </Grid>
              <Grid item></Grid>
            </Grid>
          </Toolbar>
        </AppBar>
        <AppBar component="div" className="secondary-bar" color="primary" position="static" elevation={0}>
          {this.props.tabs}
        </AppBar>
      </React.Fragment>
    )
  }
}

export default HeaderBase
