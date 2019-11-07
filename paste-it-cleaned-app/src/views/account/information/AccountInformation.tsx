import React from "react"
import { createStyles, Theme, withStyles, WithStyles } from "@material-ui/core/styles"

const styles = (theme: Theme) =>
  createStyles({
    contentWrapper: {
      margin: "40px 16px"
    }
  })

export interface AccountInformationProps extends WithStyles<typeof styles> {}

class AccountInformation extends React.Component<AccountInformationProps> {
  render() {
    const { classes } = this.props

    return <div>asdf</div>
  }
}

export default withStyles(styles)(AccountInformation)
