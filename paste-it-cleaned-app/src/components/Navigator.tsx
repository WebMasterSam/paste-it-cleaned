import React from "react"
import { Link, Switch, Route, NavLink } from "react-router-dom"
import clsx from "clsx"

import { createStyles, Theme, withStyles, WithStyles } from "@material-ui/core/styles"
import { Omit } from "@material-ui/types"

import Divider from "@material-ui/core/Divider"
import Drawer, { DrawerProps } from "@material-ui/core/Drawer"
import List from "@material-ui/core/List"
import ListItem from "@material-ui/core/ListItem"
import ListItemIcon from "@material-ui/core/ListItemIcon"
import ListItemText from "@material-ui/core/ListItemText"
import HomeIcon from "@material-ui/icons/Home"
import SettingsIcon from "@material-ui/icons/Settings"
import FileCopyIcon from "@material-ui/icons/FileCopy"
import AccountBoxIcon from "@material-ui/icons/AccountBox"
import AssessmentIcon from "@material-ui/icons/Assessment"
import VpnKeyIcon from "@material-ui/icons/VpnKey"
import CreditCardIcon from "@material-ui/icons/CreditCard"

const categories = [
  {
    id: "Account",
    children: [{ id: "Information", path: "/account", icon: <AccountBoxIcon /> }, { id: "Billing", path: "/billing", icon: <CreditCardIcon /> }]
  },
  {
    id: "Plugin",
    children: [{ id: "Api Key(s)", path: "/api-key", icon: <VpnKeyIcon /> }, { id: "Plugin Config", path: "/config", icon: <SettingsIcon /> }, { id: "Plugin Integration", path: "/integration", icon: <SettingsIcon /> }]
  },
  {
    id: "Analytics",
    children: [{ id: "Usage", path: "/usage", icon: <AssessmentIcon /> }, { id: "Latest pastes", path: "/hits", icon: <FileCopyIcon /> }]
  }
]

const styles = (theme: Theme) =>
  createStyles({
    categoryHeader: {
      paddingTop: theme.spacing(2),
      paddingBottom: theme.spacing(2)
    },
    categoryHeaderPrimary: {
      color: theme.palette.common.white
    },
    item: {
      paddingTop: 1,
      paddingBottom: 1,
      color: "rgba(255, 255, 255, 0.7)",
      "&:hover,&:focus": {
        backgroundColor: "rgba(255, 255, 255, 0.08)"
      }
    },
    itemCategory: {
      backgroundColor: "#232f3e",
      boxShadow: "0 -1px 0 #404854 inset",
      paddingTop: theme.spacing(2),
      paddingBottom: theme.spacing(2)
    },
    firebase: {
      fontSize: 24,
      color: theme.palette.common.white
    },
    itemActiveItem: {
      color: "#4fc3f7"
    },
    itemPrimary: {
      fontSize: "inherit",
      width: "100%",
      display: "block"
    },
    itemPrimaryLink: {
      color: "#bbb",
      textDecoration: "none",
      width: "100%",
      display: "block"
    },
    itemIcon: {
      minWidth: "auto",
      marginRight: theme.spacing(2)
    },
    divider: {
      marginTop: theme.spacing(2)
    }
  })

export interface NavigatorProps extends Omit<DrawerProps, "classes">, WithStyles<typeof styles> {
  current: string
}

class Navigator extends React.Component<NavigatorProps> {
  render() {
    const { classes, ...other } = this.props

    console.log(window.location.pathname)
    return (
      <Drawer variant="permanent" {...other}>
        {this.props.current}
        <List disablePadding>
          <ListItem className={clsx(classes.firebase, classes.item, classes.itemCategory)}>PasteItCleaned</ListItem>
          <ListItem className={clsx(classes.item, classes.itemCategory)}>
            <ListItemIcon className={classes.itemIcon}>
              <HomeIcon />
            </ListItemIcon>
            <ListItemText
              classes={{
                primary: classes.itemPrimary
              }}
            >
              <Link to={`/`} className={classes.itemPrimaryLink}>
                Dashboard
              </Link>
            </ListItemText>
          </ListItem>

          {categories.map(({ id, children }) => (
            <React.Fragment key={id}>
              <ListItem className={classes.categoryHeader}>
                <ListItemText
                  classes={{
                    primary: classes.categoryHeaderPrimary
                  }}
                >
                  {id}
                </ListItemText>
              </ListItem>
              {children.map(({ id: childId, path, icon }) => {
                console.log(window.location.pathname)
                console.log(path)
                var active = window.location.pathname.startsWith(path)
                return (
                  <ListItem key={childId} button className={clsx(classes.item, active && classes.itemActiveItem)}>
                    <ListItemIcon className={classes.itemIcon}>{icon}</ListItemIcon>
                    <ListItemText
                      classes={{
                        primary: classes.itemPrimary
                      }}
                    >
                      <Link to={`${path}`} className={classes.itemPrimaryLink}>
                        {childId}
                      </Link>
                    </ListItemText>
                  </ListItem>
                )
              })}
              <Divider className={classes.divider} />
            </React.Fragment>
          ))}
        </List>
      </Drawer>
    )
  }
}

export default withStyles(styles)(Navigator)
