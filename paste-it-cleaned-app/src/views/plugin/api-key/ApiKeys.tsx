import React, { Fragment } from 'react'
import VpnKeyIcon from '@material-ui/icons/VpnKey'
import AddIcon from '@material-ui/icons/Add'
import RemoveIcon from '@material-ui/icons/Remove'
import DeleteIcon from '@material-ui/icons/Delete'
import EditIcon from '@material-ui/icons/Edit'

import { Paper, Typography, Grid, FormControlLabel, Switch, Chip, Button, Tooltip } from '@material-ui/core'

import './ApiKeys.less'

export interface ApiKeysProps {}

class ApiKeys extends React.Component<ApiKeysProps> {
    private handleDeleteDomain() {
        // gérer la clé associée
    }
    render() {
        return (
            <Fragment>
                <Typography variant="h2" className="override-h2" component="h2">
                    <VpnKeyIcon /> Api keys
                </Typography>
                <br />
                <Button variant="contained" color="primary">
                    Generate new key
                </Button>
                <Paper className="paper paper-no-padding">
                    <Grid container spacing={1}>
                        <Grid item xs={12} md={2} className="api-key-card-left grid-paper-padding">
                            <Grid container direction="column" justify="space-between" alignItems="flex-start" style={{ height: '100%' }}>
                                <Grid item>
                                    <Chip size="small" label="Active" className="chip-green" />
                                </Grid>
                                <Grid item>
                                    <Typography variant="caption" className="override-caption" component="span" style={{ color: '#ddd' }}>
                                        Expires on{' '}
                                        <span style={{ whiteSpace: 'nowrap' }}>
                                            2019-02-15 <EditIcon onClick={() => {}} cursor="pointer" />
                                        </span>
                                    </Typography>
                                </Grid>
                                <Grid item>
                                    <Button color="secondary" size="small" variant="contained">
                                        <RemoveIcon /> Delete key
                                    </Button>
                                </Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} md={10} className="grid-paper-padding">
                            <Grid container direction="column" justify="space-between" alignItems="flex-start" style={{ height: '100%' }}>
                                <Grid item style={{ height: '35px', width: '100%' }}>
                                    <Grid container justify="space-between" alignItems="flex-start" style={{ width: '100%', display: 'flex' }}>
                                        <Grid item style={{ verticalAlign: 'middle' }}>
                                            <Typography variant="h3" className="override-h2" component="h3">
                                                <VpnKeyIcon />
                                                2354b4968e774dbe8
                                            </Typography>
                                        </Grid>
                                        <Grid item style={{ alignSelf: 'flex-start', paddingRight: '5px' }}>
                                            <FormControlLabel labelPlacement="start" control={<Switch checked={true} value="checkedB" color="primary" size="small" />} label="Enabled" />
                                        </Grid>
                                    </Grid>
                                </Grid>
                                <Grid item>
                                    <Chip size="small" variant="default" color="primary" label="commerscale.com" className="chip-spaced" deleteIcon={<DeleteIcon />} onDelete={this.handleDeleteDomain} />
                                    <Chip size="small" variant="default" color="primary" label="asdf.com" className="chip-spaced" deleteIcon={<DeleteIcon />} onDelete={this.handleDeleteDomain} />
                                    <Chip size="small" variant="default" color="primary" label="fffff.com" className="chip-spaced" deleteIcon={<DeleteIcon />} onDelete={this.handleDeleteDomain} />
                                    <Chip size="small" variant="default" color="primary" label="eeeeee.com" className="chip-spaced" deleteIcon={<DeleteIcon />} onDelete={this.handleDeleteDomain} />
                                    <Chip size="small" variant="default" color="primary" label="sasdfewefsdg.com" className="chip-spaced" deleteIcon={<DeleteIcon />} onDelete={this.handleDeleteDomain} />
                                </Grid>
                                <Grid item style={{ height: '45px', display: 'flex' }}>
                                    <Button color="primary" size="small" variant="outlined" style={{ alignSelf: 'flex-end' }}>
                                        <AddIcon /> Add domain
                                    </Button>
                                </Grid>
                            </Grid>
                        </Grid>
                    </Grid>
                </Paper>
            </Fragment>
        )
    }
}

export default ApiKeys
