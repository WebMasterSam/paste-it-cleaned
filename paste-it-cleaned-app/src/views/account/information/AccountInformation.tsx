import React from "react"
import { TextField, Paper, Grid, Button, Typography } from "@material-ui/core"

import "./AccountInformation.less"

export interface AccountInformationProps {}

class AccountInformation extends React.Component<AccountInformationProps> {
  render() {
    return (
      <React.Fragment>
        <Paper className="paper">
          <Typography variant="h2" component="h2">
            Contact information
          </Typography>

          <Grid container spacing={3}>
            <Grid item xs={4}>
              <TextField name="firstName" label="First Name" fullWidth />
            </Grid>
            <Grid item xs={4}>
              <TextField name="lastName" label="Last Name" fullWidth />
            </Grid>
          </Grid>

          <Grid container spacing={3}>
            <Grid item xs={4}>
              <TextField name="email" label="Email" fullWidth />
            </Grid>
          </Grid>

          <Grid container spacing={3}>
            <Grid item xs={4}>
              <Button variant="contained" type="submit" color="primary" disabled={false}>
                Update
              </Button>
            </Grid>
          </Grid>
        </Paper>

        <Paper className="paper">
          <Typography variant="h2" component="h2">
            Business information
          </Typography>

          <Grid container spacing={3}>
            <Grid item xs={4}>
              <TextField name="firstName" label="First Name" />
            </Grid>
            <Grid item xs={4}>
              <TextField name="lastName" label="Last Name" />
            </Grid>
          </Grid>

          <Grid container spacing={3}>
            <Grid item xs={4}>
              <TextField name="email" label="Email" />
            </Grid>
          </Grid>

          <Grid container spacing={3}>
            <Grid item xs={4}>
              <Button variant="contained" type="submit" color="primary" disabled={false}>
                Update
              </Button>
            </Grid>
          </Grid>
        </Paper>
        <pre>
          {JSON.stringify({
            Business: {
              Contact: {
                Address: "-",
                City: "-",
                Country: "-",
                FirstName: "-",
                LastName: "-",
                PhoneNumber: "-",
                State: "-"
              },
              Name: "Weblex"
            },
            Contact: {
              Address: "-",
              City: "-",
              Country: "-",
              FirstName: "-",
              LastName: "-",
              PhoneNumber: "-",
              State: "-"
            }
          })}
        </pre>
      </React.Fragment>
    )
  }
}

export default AccountInformation
