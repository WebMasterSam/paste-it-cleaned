import React from "react"
import Link from "@material-ui/core/Link"
import Typography from "@material-ui/core/Typography"
import { createStyles, Theme, withStyles, WithStyles } from "@material-ui/core/styles"

const styles = (theme: Theme) =>
  createStyles({
    footer: {
      padding: theme.spacing(2),
      background: "#eaeff1"
    }
  })

interface FooterProps extends WithStyles<typeof styles> {}

function Copyright() {
  return (
    <Typography variant="body2" color="textSecondary" align="center">
      {"Copyright © "}
      <Link color="inherit" href="https://pasteitcleaned.io/">
        PasteItCleaned
      </Link>{" "}
      {new Date().getFullYear()}
    </Typography>
  )
}

class Footer extends React.Component<FooterProps> {
  render() {
    const { classes } = this.props

    return (
      <footer className={classes.footer}>
        <Copyright />
      </footer>
    )
  }
}

export default withStyles(styles)(Footer)
